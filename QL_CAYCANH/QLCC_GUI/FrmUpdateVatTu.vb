Imports QLCC_BUS
Imports QLCC_DTO
Imports Utility
Imports System.Data.SqlClient
Imports System.Data.DataTable
Imports System.Configuration

Public Class FrmUpdateVatTu
    'Dim connection As New SqlConnection("Data Source=DESKTOP-SNJJV8M;Initial Catalog=QLCC;Integrated Security=True")
    ' Read ConnectionString value from App.config file
    Dim connection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))

    Dim table As New DataTable("Table")
    Dim index As Integer
    Public Sub FiterData(valSearch As String)
        Dim searchQuery As String = "SELECT * FROM [VATTU] WHERE CONCAT (ID_VATTU,TEN_VATTU,ID_DONVI) LIKE'%" & tbx_search.Text & "%'"
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

    Private Sub FrmUpdateVatTu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FiterData("")

        ' Load loai cay list
        Dim command As New SqlCommand("SELECT * FROM [DONVI]", connection)
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)
        cbx_Donvi.DataSource = table
        cbx_Donvi.DisplayMember = "TEN_DONVI"
        cbx_Donvi.ValueMember = "ID_DONVI"
    End Sub

    Private Sub btn_search_Click(sender As Object, e As EventArgs) Handles btn_search.Click
        FiterData(tbx_search.Text)
    End Sub

    Private Sub tbx_search_TextChanged(sender As Object, e As EventArgs) Handles tbx_search.TextChanged
        FiterData(tbx_search.Text)
    End Sub

    Private Sub btn_del_Click(sender As Object, e As EventArgs) Handles btn_del.Click
        If DataGridView1.Rows(0).IsNewRow Then
        Else
            DataGridView1.Rows.RemoveAt(index)
            Dim deleteQuery As String = "DELETE FROM [VATTU] WHERE [ID_VATTU] = '" & tbx_IDVatTu.Text & "'"
            ExcuteQuery(deleteQuery)
            MessageBox.Show("Delete successful supplies.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DataGridView1.Refresh()
            FiterData("")
        End If
    End Sub

    Private Sub btn_up_Click(sender As Object, e As EventArgs) Handles btn_up.Click
        If DataGridView1.Rows(0).IsNewRow Then
        Else
            Dim updateQuery As String = "UPDATE [VATTU] SET [ID_VATTU] = '" & tbx_IDVatTu.Text & "'
                                                            ,[TEN_VATTU] = '" & tbx_TenVatTu.Text & "'
                                                            ,[ID_DONVI] = '" & Convert.ToInt32(cbx_Donvi.SelectedValue) & "'
                                                            WHERE [ID_VATTU] = '" & tbx_IDVatTu.Text & "'"
            ExcuteQuery(updateQuery)
            MessageBox.Show("Update successful supplies.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DataGridView1.Refresh()
            FiterData("")
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
            tbx_IDVatTu.Text = SelectRow.Cells(0).Value.ToString()
            tbx_TenVatTu.Text = SelectRow.Cells(1).Value.ToString()
            cbx_Donvi.Text = SelectRow.Cells(2).Value.ToString()
        End If
    End Sub
End Class