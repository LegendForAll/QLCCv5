Imports QLCC_BUS
Imports QLCC_DTO
Imports Utility
Imports System.Data.SqlClient
Imports System.Configuration

Public Class FrmThemCay
    Private cayBus As CayBUS
    Private vitri As ViTriCayBUS
    'Dim connection As New SqlConnection("Data Source=DESKTOP-SNJJV8M;Initial Catalog=QLCC;Integrated Security=True")
    ' Read ConnectionString value from App.config file
    Dim connection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))

    Private Sub FrmThemCay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cayBus = New CayBUS()
        vitri = New ViTriCayBUS()

        ' Load loai cay list

        Dim command As New SqlCommand("SELECT * FROM [LOAICAY]", connection)
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)
        cbx_LoaiCay.DataSource = table
        cbx_LoaiCay.DisplayMember = "TEN_LOAICAY"
        cbx_LoaiCay.ValueMember = "ID_LOAICAY"

        ' Load vi tri cay list

        Dim command_vt As New SqlCommand("SELECT * FROM [VITRI]", connection)
        Dim adapter_vt As New SqlDataAdapter(command_vt)
        Dim table_vt As New DataTable()
        adapter_vt.Fill(table_vt)
        cbx_ViTri.DataSource = table_vt
        cbx_ViTri.DisplayMember = "TEN_VITRI"
        cbx_ViTri.ValueMember = "ID_VITRI"

        'set MSSH auto
        Dim result As Result
        Dim nextMshs = "1"
        result = CayBUS.buildMS_Cay(nextMshs)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code tree failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDCay.Text = nextMshs
    End Sub

    Private Sub btn_ThemCay_Click(sender As Object, e As EventArgs) Handles btn_ThemCay.Click
        Dim Cay As CayDTO
        Cay = New CayDTO()

        '1. Mapping data from GUI control
        Cay.MS_Cay = tbx_IDCay.Text
        Cay.TenCay = tbx_TenCay.Text
        Cay.MS_LoaiCay = Convert.ToString(cbx_LoaiCay.SelectedValue)
        Cay.ViTriCay = Convert.ToString(cbx_ViTri.SelectedValue)
        Cay.NgayTrong = dtp_NgayTrong.Value

        '2. Business .....
        If (cayBus.isValidName(Cay) = False) Then
            MessageBox.Show("Tree name is not correct")
            tbx_TenCay.Focus()
            Return
        End If
        '3. Insert to DB
        Dim result As Result
        result = cayBus.insert(Cay)
        If (result.FlagResult = True) Then
            MessageBox.Show("Add tree Success.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'set MSSH auto
            Dim nextMshs = "1"
            result = cayBus.buildMS_Cay(nextMshs)
            If (result.FlagResult = False) Then
                MessageBox.Show("Get the autocomplete code tree failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            tbx_IDCay.Text = nextMshs
            tbx_TenCay.Text = String.Empty

        Else
            MessageBox.Show("Add tree unsuccessful.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
        End If
    End Sub

    Private Sub btn_huy_Click(sender As Object, e As EventArgs) Handles btn_huy.Click
        Me.Close()
    End Sub
End Class