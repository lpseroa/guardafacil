using System;
using System.IO;
using System.Runtime.CompilerServices; // Necessário para os atributos de chamador

namespace GuardaFacil
{
  /// <summary>
  /// Classe estática responsável pelo gerenciamento de logs do sistema.
  /// Implementa gravação em arquivo com suporte a rotação automática por tamanho.
  /// </summary>
  /// 


  /// Exemplo de uso com a severidade Info
  /// Logger.Info("O aplicativo foi iniciado com sucesso.");
  public static class Logger
  {
    /// <summary>Caminho completo do arquivo de log principal.</summary>
    private static string caminhoLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GuardaFacil.log");

    /// <summary>Limite de tamanho do arquivo antes de realizar a rotação (1 MB).</summary>
    private static long TAMANHO_MAXIMO = 1024 * 1024; // 1 MB em bytes

    /// <summary>
    /// Grava o log capturando automaticamente a origem da chamada.
    /// </summary>
    /// <param name="mensagem">Mensagem do log.</param>
    /// <param name="nivel">Nível (INFO, ERRO, etc).</param>
    /// <param name="arquivo">Preenchido automaticamente pelo compilador.</param>
    /// <param name="linha">Preenchido automaticamente pelo compilador.</param>
    public static void Log(
        string mensagem,
        string nivel = "INFO",
        [CallerMemberName] string metodo = "",
        [CallerFilePath] string arquivo = "",
        [CallerLineNumber] int linha = 0)
    {
      try
      {
        VerificarTamanhoArquivo();

        // Extrai apenas o nome do arquivo (sem o caminho completo do seu PC)
        string nomeArquivo = Path.GetFileName(arquivo);

        string linhaLog = $"[{DateTime.Now:HH:mm:ss}] [{nivel}] [{nomeArquivo} -> {metodo}:{linha}] {mensagem}";

        lock (caminhoLog)
        {
          File.AppendAllText(caminhoLog, linhaLog + Environment.NewLine);
        }
      }
      catch { }
    }

    // É vital que esses métodos também tenham os atributos nos parâmetros
    // para que eles capturem a linha de QUEM OS CHAMOU e repassem para o método Log.


    public static void Sucesso(string msg, [CallerMemberName] string m = "", [CallerFilePath] string f = "", [CallerLineNumber] int l = 0)
      => Log(msg, "SUCESSO", m, f, l);

    public static void Info(string msg, [CallerMemberName] string m = "", [CallerFilePath] string f = "", [CallerLineNumber] int l = 0)
      => Log(msg, "INFO", m, f, l);

    public static void Alerta(string msg, [CallerMemberName] string m = "", [CallerFilePath] string f = "", [CallerLineNumber] int l = 0)
      => Log(msg, "AVISO", m, f, l);

    public static void Erro(string msg, [CallerMemberName] string m = "", [CallerFilePath] string f = "", [CallerLineNumber] int l = 0)
      => Log(msg, "ERRO", m, f, l);
    // ... (Mantenha o método VerificarTamanhoArquivo como está)


    /// <summary>
    /// Verifica se o arquivo de log atingiu o limite de tamanho definido.
    /// Caso positivo, move o arquivo atual para um backup (.old) e inicia um novo.
    /// </summary>
    /// <exception cref="IOException">Lançada se houver falha ao mover ou deletar arquivos.</exception>
    private static void VerificarTamanhoArquivo()
    {
      try
      {
        FileInfo info = new FileInfo(caminhoLog);

        // Se o arquivo existir e for maior que 1MB
        if (info.Exists && info.Length > TAMANHO_MAXIMO)
        {
          string backupPath = caminhoLog + ".old";

          // Remove o histórico anterior para dar lugar ao novo backup
          if (File.Exists(backupPath)) File.Delete(backupPath);

          // Renomeia o log atual para .old e libera o caminho para um novo arquivo limpo
          File.Move(caminhoLog, backupPath);

          Log("Arquivo de log rotacionado (atingiu 1MB).", "SISTEMA");
        }
      }
      catch { }
    }
  }
}