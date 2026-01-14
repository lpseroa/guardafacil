using System;
using System.Windows.Forms;

namespace GuardaFacil;

/// <summary>
/// Classe principal da interface do usuário do aplicativo GuardaFacil.
/// Gerencia a exibição do histórico, monitoramento da área de transferência e ícone de bandeja.
/// </summary>
public partial class Form1 : Form
{
  /// <summary>Instância do monitor que escuta eventos de mudança no Clipboard do Windows.</summary>
  private ClipboardMonitor _monitor;

  /// <summary>Sinalizador (flag) usado para evitar loops infinitos quando o app copia dados para o sistema.</summary>
  private bool _ignorarProximaCaptura = false;

  /// <summary>Armazena o conteúdo da última captura bem-sucedida para evitar duplicatas idênticas.</summary>
  private string _ultimoTextoCapturado = "";

  /// <summary>Registra o horário da última captura para implementar a lógica de debounce (travamento por tempo).</summary>
  private DateTime _ultimaCapturaTime = DateTime.MinValue;

  /// <summary>
  /// Inicializa uma nova instância da classe <see cref="Form1"/>.
  /// Configura componentes, carrega dados persistidos e inicia o monitoramento.
  /// </summary>
  public Form1()
  {


    InitializeComponent();

    // Carrega o que está no arquivo JSON e mostra na tela
    ClipboardBuffer.CarregarDados();
    AtualizarInterface();

    _monitor = new ClipboardMonitor();
    _monitor.ClipboardContentChanged += OnClipboardChanged;
  }

  /// <summary>
  /// Manipulador de evento disparado quando o conteúdo da área de transferência do Windows muda.
  /// </summary>
  /// <param name="sender">O objeto que disparou o evento.</param>
  /// <param name="e">Argumentos do evento.</param>
  private void OnClipboardChanged(object? sender, EventArgs e)
  {
    Logger.Info("OnClipboardChanged");


    // Verificação da Flag de clique interno
    if (_ignorarProximaCaptura)
    {
      _ignorarProximaCaptura = false;
      return;
    }

    // Trava de Tempo (Debounce): Ignora disparos que acontecem em menos de 300ms
    // Isso mata as duplicatas geradas pelo próprio Windows ao copiar
    if ((DateTime.Now - _ultimaCapturaTime).TotalMilliseconds < 300)
    {
      return;
    }

    // Pausa técnica para garantir que o SO finalizou a escrita no Clipboard
    System.Threading.Thread.Sleep(150);

    if (Clipboard.ContainsText())
    {
      string texto = Clipboard.GetText().Trim();

      // Verificação de Conteúdo: Se for igual ao último, ignora
      if (texto == _ultimoTextoCapturado) return;

      _ultimoTextoCapturado = texto;
      _ultimaCapturaTime = DateTime.Now;

      // Garante que a atualização da UI ocorra na Thread principal
      this.BeginInvoke(new Action(() =>
      {
        ClipboardBuffer.AddItem(texto);
        AtualizarInterface();
      }));
    }

  }

  /// <summary>
  /// Sobrescreve o comportamento de fechamento do formulário para ocultá-lo na bandeja em vez de encerrar.
  /// </summary>
  /// <param name="e">Argumentos do evento de fechamento.</param>
  protected override void OnFormClosing(FormClosingEventArgs e)
  {
    Logger.Info("OnFormClosing");

    if (e.CloseReason == CloseReason.UserClosing)
    {
      e.Cancel = true;
      this.Hide();
    }
    base.OnFormClosing(e);
  }


  /// <summary>
  /// Restaura a visibilidade da janela principal ao receber um clique duplo no ícone da bandeja.
  /// </summary>
  private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
  {
    Logger.Info("notifyIcon1_MouseDoubleClick");

    this.Show();
    this.WindowState = FormWindowState.Normal;
    AtualizarInterface(); // Força a lista a ler os dados da memória de novo
  }

  /// <summary>
  /// Finaliza completamente a aplicação através do menu de contexto da bandeja.
  /// </summary>
  private void sairToolStripMenuItem_Click(object sender, EventArgs e)
  {
    Logger.Info("sairToolStripMenuItem_Click");

    _monitor.Dispose();
    Application.Exit();
  }

  /// <summary>
  /// Evento disparado no carregamento inicial do formulário (usado para logs de diagnóstico).
  /// </summary>
  private void Form1_Load(object sender, EventArgs e)
  {
    Logger.Info("Form1_Load");
    //ClipboardBuffer.CarregarDados(); // Carrega o que foi salvo antes
    //AtualizarInterface();           // Mostra na tela
  }


  /// <summary>
  /// Remove um item específico selecionado na ListBox através do menu de contexto.
  /// </summary>
  private void limparToolStripMenuItem_Click(object sender, EventArgs e)
  {
    Logger.Info("limparToolStripMenuItem_Click");

    // 1. Verifica se existe algo selecionado na lista
    if (lb_Historia.SelectedItem != null)
    {
      // 2. Converte o objeto selecionado para o seu tipo ClipboardItem
      var itemParaRemover = (ClipboardItem)lb_Historia.SelectedItem;

      // 3. Chama o método de remover apenas um item (que criamos no ClipboardBuffer)
      ClipboardBuffer.RemoveItem(itemParaRemover);

      // 4. Atualiza a interface para refletir a mudança
      AtualizarInterface();

      this.Text = "Item removido";
    }
  }


  /// <summary>
  /// Manipula o clique do mouse na lista de histórico para realizar a cópia do item selecionado.
  /// </summary>
  /// <param name="sender">A ListBox de histórico.</param>
  /// <param name="e">Dados do clique do mouse.</param>
  private void lb_Historia_MouseUp(object sender, MouseEventArgs e)
  {
    Logger.Info("lb_Historia_MouseUp");

    if (lb_Historia.SelectedItem != null && e.Button == MouseButtons.Left)
    {

      Logger.Alerta("lb_Historia_MouseUp");

      var item = (ClipboardItem)lb_Historia.SelectedItem;

      _ignorarProximaCaptura = true;
      _ultimoTextoCapturado = item.Content.Trim(); // Sincroniza o último texto
      _ultimaCapturaTime = DateTime.Now;           // Sincroniza o tempo

      Clipboard.SetText(item.Content);
      this.Text = "Busca Fácil 1.0";
    }
  }

  /// <summary>
  /// Atualiza o DataSource da ListBox para refletir o estado atual do buffer de memória.
  /// </summary>
  private void AtualizarInterface()
  {
    Logger.Info("AtualizarInterface");

    // Verificação de segurança para evitar erros se a lista for nula
    lb_Historia.DataSource = null;
    lb_Historia.DataSource = ClipboardBuffer.GetHistory();
  }

  /// <summary>
  /// Solicita confirmação ao usuário e limpa todo o histórico de capturas.
  /// </summary>
  private void btn_limpar_Click(object sender, EventArgs e)
  {
    Logger.Info("btn_limpar_Click");

    var resultado = MessageBox.Show("Deseja apagar TODO o histórico?", "Confirmação", MessageBoxButtons.YesNo);
    if (resultado == DialogResult.Yes)
    {
      ClipboardBuffer.ClearHistory();
      AtualizarInterface();
    }
  }

  /// <summary>
  /// Trata os atalhos de teclado disparados quando a ListBox de histórico está em foco.
  /// Implementa as funcionalidades de cópia silenciosa (CTRL+C) e recorte/remoção (CTRL+X).
  /// </summary>
  /// <param name="sender">O objeto que disparou o evento (lb_Historia).</param>
  /// <param name="e">Argumentos contendo a tecla pressionada e modificadores (Ctrl, Shift, Alt).</param>
  /// <remarks>
  /// A lógica de teclas utiliza <see cref="KeyEventArgs.SuppressKeyPress"/> para impedir que o sistema operacional 
  /// processe o comando padrão (como o som de alerta ou comportamentos nativos da ListBox).
  /// 
  /// Comportamentos implementados:
  /// - **CTRL + C**: Reaproveita a lógica de <see cref="copiarToolStripMenuItem_Click"/> para copiar sem alterar a ordem da lista.
  /// - **CTRL + X**: Realiza o "Recorte Técnico" — copia o dado para o Clipboard do Windows e remove o item permanentemente do histórico.
  /// </remarks>
  private void lb_Historia_KeyDown(object sender, KeyEventArgs e)
  {
    // Validação defensiva para garantir que existe um item alvo para a operação
    if (lb_Historia.SelectedItem == null) return;

    // --- Lógica CTRL + C (Cópia Persistente) ---
    if (e.Control && e.KeyCode == Keys.C)
    {
      // Cancela a propagação do evento para evitar o som de "ding" do Windows
      e.SuppressKeyPress = true;

      // Reutiliza o método existente para manter a consistência da lógica de negócio
      copiarToolStripMenuItem_Click(sender, e);

      Logger.Info("Atalho acionado: CTRL+C (Copiar apenas)");
    }

    // --- Lógica CTRL + X (Retirada e Cópia) ---
    else if (e.Control && e.KeyCode == Keys.X)
    {
      e.SuppressKeyPress = true;

      var item = (ClipboardItem)lb_Historia.SelectedItem;

      // ETAPA 1: Sincronização e Cópia
      // Necessário para que o usuário possa "colar" o item que acabou de retirar da lista
      _ignorarProximaCaptura = true;
      _ultimoTextoCapturado = item.Content.Trim();
      _ultimaCapturaTime = DateTime.Now;
      Clipboard.SetText(item.Content);

      // ETAPA 2: Persistência de Dados
      // Remove o item da memória (_listaItens) e atualiza o arquivo historico.json
      ClipboardBuffer.RemoveItem(item);

      // ETAPA 3: Atualização da Interface do Usuário (UI)
      AtualizarInterface();

      this.Text = "Item retirado e copiado!";
      Logger.Info("Atalho acionado: CTRL+X (Retirar da lista)");
    }
  }

  /// <summary>
  /// Manipula o evento de clique do item "Copiar" no menu de contexto.
  /// Realiza a transferência do texto selecionado para a Área de Transferência do Windows 
  /// aplicando travas de segurança para evitar redundância no monitoramento.
  /// </summary>
  /// <param name="sender">O objeto que disparou o evento (ToolStripMenuItem).</param>
  /// <param name="e">Os argumentos do evento de clique.</param>
  /// <remarks>
  /// Este método executa as seguintes etapas críticas:
  /// 1. Validação de seleção na ListBox.
  /// 2. Configuração de sinalizadores (<see cref="_ignorarProximaCaptura"/>) para que o monitor 
  ///    do aplicativo não capture a própria ação de cópia como um novo item.
  ///    
  /// 3. Uso de <see cref="Clipboard.SetDataObject(object, bool)"/> com o parâmetro 'copy' como true, 
  ///    garantindo que o conteúdo persista no Windows após o encerramento do processo.
  /// 4. Registro de logs de sucesso ou erro para auditoria técnica.
  /// </remarks>
  private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
  {
    // 1. Verifica se realmente há um item selecionado para evitar NullReferenceException
    if (lb_Historia.SelectedItem == null)
    {
      Logger.Alerta("Tentativa de cópia sem item selecionado.");
      return;
    }

    try
    {
      // Converte o objeto da lista para o tipo ClipboardItem
      var item = (ClipboardItem)lb_Historia.SelectedItem;
      string textoParaCopiar = item.Content;

      // Validação de segurança para strings vazias
      if (string.IsNullOrEmpty(textoParaCopiar)) return;

      // 2. Sincronização de travas do monitor:
      // Informamos ao sistema que a próxima mudança no Clipboard foi gerada por nós mesmos.
      _ignorarProximaCaptura = true;
      _ultimoTextoCapturado = textoParaCopiar.Trim();
      _ultimaCapturaTime = DateTime.Now;

      // 3. Execução da cópia:
      // O parâmetro 'true' mantém os dados no Clipboard mesmo se o app for fechado.
      Clipboard.SetDataObject(textoParaCopiar, true);

      // Registro de sucesso e feedback visual para o usuário
      Logger.Sucesso($"Texto copiado via menu: {item.ToString()}");
      this.Text = "Copiado com sucesso!";
    }
    catch (Exception ex)
    {
      // Captura falhas de acesso (ex: Clipboard bloqueado por outro processo)
      Logger.Erro($"Falha ao copiar para o Clipboard: {ex.Message}");
      MessageBox.Show("Erro ao copiar para a área de transferência.", "Erro de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }
}