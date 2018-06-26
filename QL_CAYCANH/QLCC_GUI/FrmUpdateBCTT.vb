Imports QLCC_BUS
Imports QLCC_DTO
Imports Utility
Imports System.Data.SqlClient
Imports System.Data.DataTable
Imports System.Configuration

Public Class FrmUpdateBCTT
    'Dim connection As New SqlConnection("Data Source=DESKTOP-SNJJV8M;Initial Catalog=QLCC;Integrated Security=True")
    ' Read ConnectionString value from App.config file
    Dim connection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
    Dim table As New DataTable("Table")
    Dim index As Integer
    Public Sub FiterData(valSearch As String)
        Dim searchQuery As String = "SELECT DISTINCT  CT.ID_BCCT_TINHTRANGCAY, CT.ID_BCTT_CAY, C.TENCAY, L.TEN_LOAICAY, C.NGAYTRONG, CS.TT_CAY FROM [BCCT_TINHTRANGCAY] CT,[CHAMSOC_CAY] CS, [CAY] C, [LOAICAY] L
        WHERE CT.ID_CAY = CS.ID_CAY 
        AND CT.ID_CAY=C.ID_CAY
        AND C.ID_LOAICAY=L.ID_LOAICAY
        AND CONCAT (CT.ID_BCCT_TINHTRANGCAY, CT.ID_BCTT_CAY, C.TENCAY, L.TEN_LOAICAY, C.NGAYTRONG, CS.TT_CAY) LIKE'%" & tbx_search.Text & "%'
        ORDER BY  CT.ID_BCCT_TINHTRANGCAY"

        Dim command As New SqlCommand(searchQuery, connection)
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)
        DataGridView1.AutoGenerateColumns = True
        DataGridView1.DataSource = table

    End Sub
    Public Sub FiterData1(valSearch As String)
        Dim searchQuery As String = "SELECT * FROM [BCTINHTRANG_CAY] "
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
    Private Sub FrmUpdateBCTT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FiterData("")
        FiterData1("")
    End Sub

    Private Sub btn_huy_Click(sender As Object, e As EventArgs) Handles btn_huy.Click
        Me.Close()
    End Sub

    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        If DataGridView1.Rows(0).IsNewRow Then
        Else
            DataGridView1.Rows.RemoveAt(index)
            Dim deleteQuery As String = "DELETE FROM [BCCT_TINHTRANGCAY] WHERE [ID_BCCT_TINHTRANGCAY] = '" & tbx_IDBCCT.Text & "'"
            ExcuteQuery(deleteQuery)
            MessageBox.Show("Delete category Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DataGridView1.Refresh()
            FiterData("")
        End If
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        index = e.RowIndex
        Dim SelectRow As DataGridViewRow
        If (index >= 0) Then
            SelectRow = DataGridView1.Rows(index)
            tbx_IDBCCT.Text = SelectRow.Cells(0).Value.ToString()
            tbx_IDBC.Text = SelectRow.Cells(1).Value.ToString()
        End If
    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        FiterData("")
    End Sub

    Private Sub DataGridView2_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView2.CellMouseClick
        index = e.RowIndex
        Dim SelectRow As DataGridViewRow
        If (index >= 0) Then
            SelectRow = DataGridView2.Rows(index)
            tbx_IDLich.Text = SelectRow.Cells(0).Value.ToString()
            dtp_TGLich.Value = Convert.ToDateTime(SelectRow.Cells(1).Value.ToString())
        End If
    End Sub

    Private Sub btn_de2_Click(sender As Object, e As EventArgs) Handles btn_de2.Click
        If DataGridView2.Rows(0).IsNewRow Then
        Else
            DataGridView2.Rows.RemoveAt(index)
            Dim deleteQuery As String = "DELETE FROM [BCTINHTRANG_CAY] WHERE [ID_BCTT_CAY] = '" & tbx_IDLich.Text & "'"
            ExcuteQuery(deleteQuery)
            Dim deleteQuery1 As String = "DELETE FROM [BCCT_TINHTRANGCAY] WHERE [ID_BCTT_CAY] = '" & tbx_IDLich.Text & "'"
            ExcuteQuery(deleteQuery1)

            MessageBox.Show("Delete calender report Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DataGridView2.Refresh()
            FiterData1("")
            FiterData("")
        End If
    End Sub

    Private Sub tbx_search_TextChanged(sender As Object, e As EventArgs) Handles tbx_search.TextChanged
        FiterData("")
    End Sub
End Class