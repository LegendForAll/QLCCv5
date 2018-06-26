Imports QLCC_BUS
Imports QLCC_DTO
Imports Utility
Imports System.Data.SqlClient
Imports System.Data.DataTable
Imports System.Configuration

Public Class FrmUpdateLichCS
    Private lichBus As LichChamSocBUS
    Private lichCTBus As LichChamSoc_CTBUS
    'Dim connection As New SqlConnection("Data Source=DESKTOP-SNJJV8M;Initial Catalog=QLCC;Integrated Security=True")
    ' Read ConnectionString value from App.config file
    Dim connection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))

    Dim table As New DataTable("Table")
    Dim index As Integer
    Public Sub FiterData(valSearch As String)
        'Dim searchQuery As String = "SELECT L.NGAYLAP,CS.ID_CHAMSOC, CS.TG_CHAMSOC, C.TENCAY, V.TEN_VATTU, CS.TT_CAY, CS.GHICHU
        'FROM [CHAMSOC_CAY] CS,  [VATTU] V, [CAY] C ,[LICHCHAMSOC] L, [LICHCT] CT
        'WHERE CS.ID_CAY=C.ID_CAY AND CS.ID_VATTU=V.ID_VATTU 
        'AND   L.ID_LICHCHAMSOC = CT.ID_LICHCHAMSOC
        'AND   CT.ID_CHAMSOC = CS.ID_CHAMSOC
        'AND CONCAT (L.NGAYLAP,CS.ID_CHAMSOC, CS.TG_CHAMSOC, C.TENCAY, V.TEN_VATTU, CS.TT_CAY, CS.GHICHU) LIKE'%" & tbx_search.Text & "%'"

        'Dim searchQuery As String = "SELECT CS.ID_CHAMSOC, CS.TG_CHAMSOC, C.TENCAY, V.TEN_VATTU, CS.TT_CAY, CS.GHICHU FROM [CHAMSOC_CAY] CS, [LICHCHAMSOC] L, [LICHCT] CT, [VATTU] V, [CAY] C
        'WHERE L.ID_LICHCHAMSOC = '" & tbx_IDLich.Text & "'
        'AND CS.ID_CAY=C.ID_CAY AND CS.ID_VATTU=V.ID_VATTU
        'AND L.ID_LICHCHAMSOC = CT.ID_LICHCHAMSOC
        'AND CT.ID_CHAMSOC = CS.ID_CHAMSOC"
        Dim searchQuery As String = "SELECT DISTINCT CT.ID_LICHCT, C.TENCAY, CS.TG_CHAMSOC, CS.ID_VATTU, CS.SL_CHAMSOC, CS.GHICHU
        FROM [LICHCT] CT, [CAY] C, [CHAMSOC_CAY] CS
        WHERE CS.ID_CAY = C.ID_CAY AND CT.ID_CHAMSOC = CS.ID_CHAMSOC
        AND CONCAT (CT.ID_LICHCT, C.TENCAY, CS.TG_CHAMSOC, CS.ID_VATTU, CS.SL_CHAMSOC, CS.GHICHU) LIKE'%" & tbx_search.Text & "%'
        ORDER BY  CT.ID_LICHCT"
        Dim command As New SqlCommand(searchQuery, connection)
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)

        DataGridView1.DataSource = table
    End Sub
    Public Sub FiterData1(valSearch As String)
        Dim searchQuery As String = "SELECT * FROM [LICHCHAMSOC] "
        Dim command As New SqlCommand(searchQuery, connection)
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)

        DataGridView2.DataSource = table
    End Sub
    Public Sub ExcuteQuery(query As String)
        Dim command As New SqlCommand(query, connection)
        connection.Open()
        command.ExecuteNonQuery()
        connection.Close()
    End Sub
    Private Sub FrmUpdateLichCS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FiterData("")
        FiterData1("")
    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        FiterData(tbx_search.Text)
    End Sub

    Private Sub tbx_search_TextChanged(sender As Object, e As EventArgs) Handles tbx_search.TextChanged
        FiterData(tbx_search.Text)
    End Sub

    Private Sub btn_de2_Click(sender As Object, e As EventArgs) Handles btn_de2.Click
        If DataGridView2.Rows(0).IsNewRow Then
        Else
            DataGridView2.Rows.RemoveAt(index)
            Dim deleteQuery As String = "DELETE FROM [LICHCHAMSOC] WHERE [ID_LICHCHAMSOC] = '" & tbx_IDLich.Text & "'"
            ExcuteQuery(deleteQuery)
            Dim deleteQuery1 As String = "DELETE FROM [LICHCT] WHERE [ID_LICHCHAMSOC] = '" & tbx_IDLich.Text & "'"
            ExcuteQuery(deleteQuery1)

            MessageBox.Show("Delete calender care Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DataGridView2.Refresh()
            FiterData1("")
            FiterData("")
        End If
    End Sub

    Private Sub DataGridView2_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView2.CellMouseClick
        index = e.RowIndex
        Dim SelectRow As DataGridViewRow
        If (index >= 0) Then
            SelectRow = DataGridView2.Rows(index)
            tbx_IDLich.Text = SelectRow.Cells(0).Value.ToString()
            dtp_TGLich.Text = Convert.ToDateTime(SelectRow.Cells(1).Value.ToString())
        End If
    End Sub

    Private Sub btn_huy_Click(sender As Object, e As EventArgs) Handles btn_huy.Click
        Me.Close()
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        index = e.RowIndex
        Dim SelectRow As DataGridViewRow
        If (index >= 0) Then
            SelectRow = DataGridView1.Rows(index)
            tbx_IDLichCT.Text = SelectRow.Cells(0).Value.ToString()
            tbx_TenCay.Text = SelectRow.Cells(1).Value.ToString()
            dtp_TGCScay.Text = Convert.ToDateTime(SelectRow.Cells(2).Value.ToString())
        End If
    End Sub

    Private Sub btn_de1_Click(sender As Object, e As EventArgs) Handles btn_de1.Click
        If DataGridView1.Rows(0).IsNewRow Then
        Else
            DataGridView1.Rows.RemoveAt(index)
            Dim deleteQuery1 As String = "DELETE FROM [LICHCT] WHERE [ID_LICHCT] = '" & tbx_IDLichCT.Text & "'"
            ExcuteQuery(deleteQuery1)

            MessageBox.Show("Delete calender care Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DataGridView1.Refresh()
            FiterData("")
        End If
    End Sub
End Class