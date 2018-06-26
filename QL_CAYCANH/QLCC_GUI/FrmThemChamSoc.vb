Imports QLCC_BUS
Imports QLCC_DTO
Imports Utility
Imports System.Data.SqlClient
Imports System.Configuration

Public Class FrmThemChamSoc
    Private chamsocBus As ChamSoc_CayBUS
    'Dim connection As New SqlConnection("Data Source=DESKTOP-SNJJV8M;Initial Catalog=QLCC;Integrated Security=True")
    ' Read ConnectionString value from App.config file
    Dim connection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))

    Private Sub btn_ThemCS_Click(sender As Object, e As EventArgs) Handles btn_ThemCS.Click
        Dim chamsoc As ChamSoc_CayDTO
        chamsoc = New ChamSoc_CayDTO()

        '1. Mapping data from GUI control
        chamsoc.MS_ChamSoc = tbx_IDChamSoc.Text
        chamsoc.MS_VatTu = Convert.ToString(cbx_vattu.SelectedValue)
        chamsoc.MS_Cay = Convert.ToString(cbx_TenCay.SelectedValue)
        chamsoc.TinhTrang_Cay = cbx_tinhtrang.Text
        chamsoc.GhiChu_Cay = cbx_GhiChu.Text
        chamsoc.TG_ChamSoc = dtp_TGCham.Value
        chamsoc.SoLuong_CS = Convert.ToInt32(Ud_soluongCS.Value)
        '2. Business .....

        '3. Insert to DB
        Dim result As Result
        result = chamsocBus.insert(chamsoc)
        If (result.FlagResult = True) Then
            MessageBox.Show("Add care of tree successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)


            'set ID auto
            Dim nextID = "1"
            result = chamsocBus.getNextID(nextID)
            If (result.FlagResult = False) Then
                MessageBox.Show("Get the autocomplete code care of tree failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                System.Console.WriteLine(result.SystemMessage)
                Return
            End If
            tbx_IDChamSoc.Text = nextID

        Else
            MessageBox.Show("Add care of tree unsuccessful.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
        End If
    End Sub

    Private Sub FrmThemChamSoc_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        chamsocBus = New ChamSoc_CayBUS()

        'set ID auto
        Dim result As Result
        Dim nextID = "1"
        result = chamsocBus.getNextID(nextID)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code care of tree failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDChamSoc.Text = nextID

        'connect table [VATTU]

        Dim command As New SqlCommand("SELECT * FROM [VATTU]", connection)
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)
        cbx_vattu.DataSource = table
        cbx_vattu.DisplayMember = "TEN_VATTU"
        cbx_vattu.ValueMember = "ID_VATTU"

        'connect table [CAY]

        Dim command_cay As New SqlCommand("SELECT * FROM [CAY]", connection)
        Dim adapter_cay As New SqlDataAdapter(command_cay)
        Dim table_cay As New DataTable()
        adapter_cay.Fill(table_cay)
        cbx_TenCay.DataSource = table_cay
        cbx_TenCay.DisplayMember = "TENCAY"
        cbx_TenCay.ValueMember = "ID_CAY"
    End Sub

    Private Sub btn_huy_Click(sender As Object, e As EventArgs) Handles btn_huy.Click
        Me.Close()
    End Sub
End Class