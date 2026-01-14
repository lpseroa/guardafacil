using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace GuardaFacil;

/// <summary>
/// Representa um item individual capturado da área de transferência.
/// </summary>
public class ClipboardItem
{
  /// <summary>O conteúdo de texto capturado.</summary>
  public string Content { get; set; } = "";

  /// <summary>Data e hora em que a captura foi realizada.</summary>
  public DateTime Timestamp { get; set; } = DateTime.Now;

  /// <summary>
  /// Retorna uma representação resumida do conteúdo para exibição em controles de lista.
  /// </summary>
  /// <returns>Uma string de até 80 caracteres, sem quebras de linha.</returns>
  public override string ToString()
  {
    string resumo = Content.Length > 80 ? Content.Substring(0, 80) + "..." : Content;
    return resumo.Replace("\n", " ").Replace("\r", " ");
  }
}

/// <summary>
/// Gerencia o armazenamento temporário (lista em memória) e permanente (arquivo JSON) 
/// dos itens da área de transferência.
/// </summary>
public static class ClipboardBuffer
{
  /// <summary>Lista interna que armazena os objetos <see cref="ClipboardItem"/>.</summary>
  private static List<ClipboardItem> _listaItens = new List<ClipboardItem>();

  /// <summary>Nome do arquivo onde o histórico é persistido.</summary>
  private static string caminhoArquivo = "historico.json";

  /// <summary>
  /// Adiciona um novo conteúdo ao histórico, aplicando regras de validação e limpeza.
  /// </summary>
  /// <param name="conteudo">O texto bruto capturado da área de transferência.</param>
  /// <remarks>
  /// O método realiza o Trim() no texto e verifica se ele é idêntico ao último item 
  /// adicionado para evitar duplicatas consecutivas.
  /// </remarks>
  public static void AddItem(string conteudo)
  {
    string textoLimpo = conteudo.Trim();
    if (string.IsNullOrEmpty(textoLimpo)) return;

    // PROTEÇÃO CONTRA DUPLICATAS:
    if (_listaItens.Count > 0 && _listaItens[0].Content.Trim() == textoLimpo)
    {
      return;
    }

    var novoItem = new ClipboardItem { Content = textoLimpo, Timestamp = DateTime.Now };
    _listaItens.Insert(0, novoItem);
    SalvarDados();
  }

  /// <summary>
  /// Obtém a lista completa do histórico atual.
  /// </summary>
  /// <returns>Uma lista de objetos <see cref="ClipboardItem"/>.</returns>
  public static List<ClipboardItem> GetHistory() => _listaItens;

  /// <summary>
  /// Remove um item específico da lista e atualiza o arquivo de persistência.
  /// </summary>
  /// <param name="item">O objeto <see cref="ClipboardItem"/> a ser removido.</param>
  public static void RemoveItem(ClipboardItem item)
  {
    if (item != null && _listaItens.Remove(item)) SalvarDados();
  }

  /// <summary>
  /// Remove todos os itens do histórico e limpa o arquivo JSON.
  /// </summary>
  public static void ClearHistory()
  {
    _listaItens.Clear();
    SalvarDados();
  }

  /// <summary>
  /// Serializa a lista atual para o formato JSON e salva no disco.
  /// </summary>
  public static void SalvarDados()
  {
    try
    {
      var json = JsonSerializer.Serialize(_listaItens);
      File.WriteAllText(caminhoArquivo, json);
    }
    catch { /* Erros de E/S são ignorados para manter a fluidez do app */ }
  }

  /// <summary>
  /// Carrega os dados do arquivo JSON para a memória ao iniciar o aplicativo.
  /// </summary>
  public static void CarregarDados()
  {
    if (!File.Exists(caminhoArquivo)) return;
    try
    {
      var json = File.ReadAllText(caminhoArquivo);
      _listaItens = JsonSerializer.Deserialize<List<ClipboardItem>>(json) ?? new List<ClipboardItem>();
    }
    catch { _listaItens = new List<ClipboardItem>(); }
  }
}