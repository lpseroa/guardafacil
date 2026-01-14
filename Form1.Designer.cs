namespace GuardaFacil
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      lb_Historia = new ListBox();
      removerItemToolStripMenuItem = new ContextMenuStrip(components);
      limparToolStripMenuItem = new ToolStripMenuItem();
      copiarToolStripMenuItem = new ToolStripMenuItem();
      notifyIcon1 = new NotifyIcon(components);
      contextMenuTray = new ContextMenuStrip(components);
      apagarEsseItemToolStripMenuItem = new ToolStripMenuItem();
      sairToolStripMenuItem = new ToolStripMenuItem();
      label1 = new Label();
      btn_limpar = new Button();
      removerItemToolStripMenuItem.SuspendLayout();
      contextMenuTray.SuspendLayout();
      SuspendLayout();
      // 
      // lb_Historia
      // 
      lb_Historia.ContextMenuStrip = removerItemToolStripMenuItem;
      lb_Historia.FormattingEnabled = true;
      lb_Historia.Location = new Point(8, 41);
      lb_Historia.Name = "lb_Historia";
      lb_Historia.Size = new Size(634, 284);
      lb_Historia.TabIndex = 0;
      lb_Historia.MouseUp += lb_Historia_MouseUp;
      // 
      // removerItemToolStripMenuItem
      // 
      removerItemToolStripMenuItem.ImageScalingSize = new Size(20, 20);
      removerItemToolStripMenuItem.Items.AddRange(new ToolStripItem[] { limparToolStripMenuItem, copiarToolStripMenuItem });
      removerItemToolStripMenuItem.Name = "contextMenuList";
      removerItemToolStripMenuItem.Size = new Size(189, 52);
      // 
      // limparToolStripMenuItem
      // 
      limparToolStripMenuItem.Name = "limparToolStripMenuItem";
      limparToolStripMenuItem.Size = new Size(188, 24);
      limparToolStripMenuItem.Text = "Remover (Ctl+X)";
      limparToolStripMenuItem.Click += limparToolStripMenuItem_Click;
      // 
      // copiarToolStripMenuItem
      // 
      copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
      copiarToolStripMenuItem.Size = new Size(188, 24);
      copiarToolStripMenuItem.Text = "Copiar (Ctl+C)";
      copiarToolStripMenuItem.Click += copiarToolStripMenuItem_Click;
      // 
      // notifyIcon1
      // 
      notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
      notifyIcon1.Text = "notifyIcon1";
      notifyIcon1.Visible = true;
      // 
      // contextMenuTray
      // 
      contextMenuTray.ImageScalingSize = new Size(20, 20);
      contextMenuTray.Items.AddRange(new ToolStripItem[] { apagarEsseItemToolStripMenuItem, sairToolStripMenuItem });
      contextMenuTray.Name = "contextMenuTray";
      contextMenuTray.Size = new Size(194, 52);
      // 
      // apagarEsseItemToolStripMenuItem
      // 
      apagarEsseItemToolStripMenuItem.Name = "apagarEsseItemToolStripMenuItem";
      apagarEsseItemToolStripMenuItem.Size = new Size(193, 24);
      apagarEsseItemToolStripMenuItem.Text = "Apagar Esse Item";
      // 
      // sairToolStripMenuItem
      // 
      sairToolStripMenuItem.Name = "sairToolStripMenuItem";
      sairToolStripMenuItem.Size = new Size(193, 24);
      sairToolStripMenuItem.Text = "Sair";
      sairToolStripMenuItem.Click += sairToolStripMenuItem_Click;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(8, 18);
      label1.Name = "label1";
      label1.Size = new Size(152, 20);
      label1.TabIndex = 1;
      label1.Text = "Area de Transferencia";
      // 
      // btn_limpar
      // 
      btn_limpar.Location = new Point(548, 8);
      btn_limpar.Name = "btn_limpar";
      btn_limpar.Size = new Size(94, 31);
      btn_limpar.TabIndex = 2;
      btn_limpar.Text = "Limpar";
      btn_limpar.UseVisualStyleBackColor = true;
      btn_limpar.Click += btn_limpar_Click;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(8F, 20F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.PeachPuff;
      ClientSize = new Size(649, 328);
      ContextMenuStrip = contextMenuTray;
      Controls.Add(btn_limpar);
      Controls.Add(label1);
      Controls.Add(lb_Historia);
      Icon = (Icon)resources.GetObject("$this.Icon");
      Name = "Form1";
      Text = "Busca Fácil 1.0";
      Load += Form1_Load;
      removerItemToolStripMenuItem.ResumeLayout(false);
      contextMenuTray.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private ListBox lb_Historia;
    private NotifyIcon notifyIcon1;
    private ContextMenuStrip contextMenuTray;
    private ToolStripMenuItem sairToolStripMenuItem;
    private Label label1;
    private ContextMenuStrip contextMenuList;
    private ToolStripMenuItem limparToolStripMenuItem;
    private Button btn_limpar;
    private ToolStripMenuItem apagarEsseItemToolStripMenuItem;
    private ContextMenuStrip removerItemToolStripMenuItem;
    private ToolStripMenuItem copiarToolStripMenuItem;
  }
}
