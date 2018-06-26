Imports QLCC_BUS
Imports QLCC_DTO
Imports Utility
Imports System.Data.SqlClient
Imports System.Data.DataTable
Imports System.Configuration

Public Class FrmBaoCaoTTCay
    Private BCaoBus As BCTinhTrang_CayBUS
    Private BCaoCTBus As BCCT_TinhTrangCayBUS
    'Dim connection As New SqlConnection("Data Source=DESKTOP-SNJJV8M;Initial Catalog=QLCC;Integrated Security=True")
    ' Read ConnectionString value from App.config file
    Dim connection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
    Dim table As New DataTable("Table")
    Dim index As Integer
    Public Sub FiterData(valSearch As String)
        Dim str As String = Convert.ToString(dtp_choose.Value.Month)
        Dim searchQuery As String = "SELECT C.ID_CAY AS ID, C.TENCAY AS TENCAY, L.TEN_LOAICAY AS LOAI, C.NGAYTRONG, CS.TT_CAY FROM [CHAMSOC_CAY] CS, [CAY] C, [LOAICAY] L
        WHERE  CS.ID_CAY=C.ID_CAY AND C.ID_LOAICAY=L.ID_LOAICAY
        AND MONTH(CS.TG_CHAMSOC)='" & str & "'"

        Dim command As New SqlCommand(searchQuery, connection)
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)

        DataGridView2.DataSource = table
    End Sub
    Public Sub FiterData1(valSearch As String)
        Dim searchQuery As String = "SELECT * FROM [BCTINHTRANG_CAY]"
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
    Private Sub FrmBaoCaoTTCay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BCaoBus = New BCTinhTrang_CayBUS()
        'set ID auto bao cao
        Dim result As Result
        Dim nextID = "1"
        result = BCaoBus.getNextID(nextID)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code report failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDBC.Text = nextID
    End Sub

    Private Sub btn_Insert_Click(sender As Object, e As EventArgs) Handles btn_Insert.Click
        Dim updateQuery As String = "INSERT INTO [BCTINHTRANG_CAY]
                                            ([ID_BCTT_CAY]
                                            ,[TG_BAOCAO])
                                            VALUES
                                            ('" & tbx_IDBC.Text & "','" & Convert.ToString(dtp_TGBC.Value) & "')"
        ExcuteQuery(updateQuery)
        MessageBox.Show("Add report calender successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        DataGridView1.Refresh()
        FiterData1("")

        'set ID auto bao cao
        BCaoBus = New BCTinhTrang_CayBUS()
        Dim result As Result
        Dim nextID = "1"
        result = BCaoBus.getNextID(nextID)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code report failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDBC.Text = nextID
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FiterData1("")
        'set ID auto chi tiet bao cao
        BCaoCTBus = New BCCT_TinhTrangCayBUS()
        Dim result As Result
        Dim nextID = "1"
        result = BCaoCTBus.getNextID(nextID)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code report failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDBCCT.Text = nextID
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        index = e.RowIndex
        Dim SelectRow As DataGridViewRow
        If (index >= 0) Then
            SelectRow = DataGridView1.Rows(index)
            tbx_IDlich.Text = SelectRow.Cells(0).Value.ToString()
            dtp_choose.Value = Convert.ToDateTime(SelectRow.Cells(1).Value.ToString())
            FiterData("")
        End If
    End Sub

    Private Sub btn_huy_Click(sender As Object, e As EventArgs) Handles btn_huy.Click
        Me.Close()
    End Sub

    Private Sub btn_insert2_Click(sender As Object, e As EventArgs) Handles btn_insert2.Click

        For i As Integer = DataGridView2.Rows.Count() - 1 To 0 Step -1
            Dim Insert As String
            Insert = DataGridView2.Rows(i).Cells(0).Value
            If Insert <> "" Then
                Dim updateQuery As String = "INSERT INTO [BCCT_TINHTRANGCAY]
                                                ([ID_BCCT_TINHTRANGCAY]
                                                ,[ID_BCTT_CAY]
                                                ,[ID_CAY])
                                                VALUES
                                                ('" & tbx_IDBCCT.Text & "',
                                                 '" & tbx_IDlich.Text & "',
                                                 '" & tbx_IDCay.Text & "')"
                ExcuteQuery(updateQuery)
                MessageBox.Show("Add report category successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

                'set ID auto chi tiet bao cao
                BCaoCTBus = New BCCT_TinhTrangCayBUS()
                Dim result As Result
                Dim nextID = "1"
                result = BCaoCTBus.getNextID(nextID)
                If (result.FlagResult = False) Then
                    MessageBox.Show("Get the autocomplete code report failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    System.Console.WriteLine(result.SystemMessage)
                    Return
                End If
                tbx_IDBCCT.Text = nextID

                'Xoa dong da them tranh trung lap khoa chinh
                Dim row As DataGridViewRow
                row = DataGridView2.Rows(i)
                DataGridView2.Rows.Remove(row)
            End If
        Next
    End Sub

    Private Sub DataGridView2_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView2.CellMouseClick
        index = e.RowIndex
        Dim SelectRow As DataGridViewRow
        If (index >= 0) Then
            SelectRow = DataGridView2.Rows(index)
            tbx_IDCay.Text = SelectRow.Cells(0).Value.ToString()
            tbx_TenCay.Text = SelectRow.Cells(1).Value.ToString()
            tbx_loaicay.Text = SelectRow.Cells(3).Value.ToString()
        End If
    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        Dim frm As FrmUpdateBCTT = New FrmUpdateBCTT()
        frm.Show()
    End Sub
End Class