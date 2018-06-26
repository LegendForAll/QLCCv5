Imports QLCC_BUS
Imports QLCC_DTO
Imports Utility
Imports System.Data.SqlClient
Imports System.Data.DataTable
Imports System.Configuration

Public Class FrmPhieuMuaVT
    'Dim connection As New SqlConnection("Data Source=DESKTOP-SNJJV8M;Initial Catalog=QLCC;Integrated Security=True")
    ' Read ConnectionString value from App.config file
    Dim connection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))

    Private phieuBus As PhieuMua_VTBUS
    Private donviBus As DonViBUS
    Private phieuCTBus As PhieuMuaVT_CTBUS
    Dim table As New DataTable("Table")
    Dim index As Integer

    Public Sub FiterData(valSearch As String)
        Dim searchQuery As String = "SELECT * FROM [PHIEUMUA_VT]"
        Dim command As New SqlCommand(searchQuery, connection)
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)

        DataGridView1.DataSource = table
    End Sub
    Public Sub ExcuteQuery(query As String)
        Dim command As New SqlCommand(query, connection)
        connection.Open()
        command.ExecuteNonQuery()
        connection.Close()
    End Sub
    Private Sub FrmPhieuMuaVT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        phieuBus = New PhieuMua_VTBUS()
        donviBus = New DonViBUS()
        phieuCTBus = New PhieuMuaVT_CTBUS()

        FiterData("")

        'set ID auto phieu mua
        Dim result As Result
        Dim nextID = "1"
        result = phieuBus.getNextID(nextID)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code offers failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDPhieu.Text = nextID

        'set ID auto chi tiet phieu mua
        Dim nextID_CT = "1"
        result = phieuCTBus.getNextID(nextID_CT)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code offers detail failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDPhieuCT.Text = nextID_CT

        'load vat tu
        Dim command As New SqlCommand("SELECT * FROM [VATTU]", connection)
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)
        cbx_TenVatTu.DataSource = table
        cbx_TenVatTu.DisplayMember = "TEN_VATTU"
        cbx_TenVatTu.ValueMember = "ID_VATTU"

    End Sub

    Private Sub btn_ThemDV_Click(sender As Object, e As EventArgs) Handles btn_ThemDV.Click
        If (tbx_tien.Text = "" And Convert.ToInt32(tbx_tien.Text) > 100000) Then
            MessageBox.Show("Money is not NULL or the amount exceeds 100,000.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        If (Convert.ToInt64(tbx_tien.Text) > 100000) Then
            MessageBox.Show("The amount exceeds 100,000.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        'So tien mua thoa dieu kien
        If (tbx_tien.Text <> "" And Convert.ToInt64(tbx_tien.Text) <= 100000) Then
            Dim insertQuery As String = "INSERT INTO [PHIEUMUA_VTCT]
           ([ID_PHIEUMUA_CT]
           ,[ID_PHIEUMUA_VT]
           ,[DIACHIMUA]
           ,[SOLUONGMUA]
           ,[SOTIEN]
           ,[ID_VATTU])
     VALUES
           ('" & tbx_IDPhieuCT.Text & "'
            ,'" & tbx_IDPhieu.Text & "'
           ,'" & tbx_diachi.Text & "'
            ,'" & Convert.ToString(ud_SoluongMua.Value) & "'
           ,'" & tbx_tien.Text & "'
            ,'" & Convert.ToString(cbx_TenVatTu.SelectedValue) & "')"
            ExcuteQuery(insertQuery)
            MessageBox.Show("Add offers detail successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

            'set ID auto chi tiet phieu
            phieuCTBus = New PhieuMuaVT_CTBUS()
            Dim result As Result
            Dim nextID = "1"
            result = phieuCTBus.getNextID(nextID)
            If (result.FlagResult = False) Then
                MessageBox.Show("Get the autocomplete code offers detail failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                System.Console.WriteLine(result.SystemMessage)
                Return
            End If
            tbx_IDPhieuCT.Text = nextID
        End If

    End Sub

    Private Sub btn_huy_Click(sender As Object, e As EventArgs) Handles btn_huy.Click
        Me.Close()
    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        Dim Frm As FrmUpdatePhieuMuaVT = New FrmUpdatePhieuMuaVT()
        Frm.Show()
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        index = e.RowIndex
        Dim SelectRow As DataGridViewRow
        If (index >= 0) Then
            SelectRow = DataGridView1.Rows(index)
            tbx_IDPhieu.Text = SelectRow.Cells(0).Value.ToString()
            dtp_TGMua.Value = Convert.ToDateTime(SelectRow.Cells(1).Value.ToString())
        End If
    End Sub

    Private Sub btn_insert_offers_Click(sender As Object, e As EventArgs) Handles btn_insert_offers.Click
        Dim updateQuery As String = "INSERT INTO [PHIEUMUA_VT]
                                            ([ID_PHIEUMUA_VT]
                                            ,[NGAYMUAVT])
                                            VALUES
                                            ('" & tbx_IDPhieu.Text & "','" & Convert.ToString(dtp_TGMua.Value) & "')"
        ExcuteQuery(updateQuery)
        MessageBox.Show("Add offers successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        DataGridView1.Refresh()
        FiterData("")

        'set ID auto
        phieuBus = New PhieuMua_VTBUS()
        Dim result As Result
        Dim nextID = "1"
        result = phieuBus.getNextID(nextID)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code offers failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDPhieu.Text = nextID
    End Sub
End Class