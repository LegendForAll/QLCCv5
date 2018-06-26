Imports QLCC_BUS
Imports QLCC_DTO
Imports Utility
Imports System.Data.SqlClient
Imports System.Configuration

Public Class FrmThemLichCS
    Private lichBus As LichChamSocBUS
    Private lichCTBus As LichChamSoc_CTBUS
    'Dim connection As New SqlConnection("Data Source=DESKTOP-SNJJV8M;Initial Catalog=QLCC;Integrated Security=True")
    ' Read ConnectionString value from App.config file
    Dim connection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
    Dim table As New DataTable("Table")
    Dim index As Integer

    Public Sub FiterData(valSearch As String)
        Dim searchQuery As String = "SELECT * FROM [LICHCHAMSOC]"
        Dim command As New SqlCommand(searchQuery, connection)
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)

        DataGridView1.DataSource = table
    End Sub
    Public Sub FiterData2(valSearch As String)
        Dim searchQuery As String = "SELECT ID_CHAMSOC, TG_CHAMSOC, ID_VATTU, SL_CHAMSOC, TT_CAY, GHICHU FROM [CHAMSOC_CAY] 
        WHERE [ID_CAY] = '" & cbx_TenCay.SelectedValue & "'
        AND [TG_CHAMSOC] >= '" & dtp_TGLich.Value & "'"

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
    Private Sub FrmThemLichCS_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        FiterData("")
        'connect table [CAY]

        Dim command_cay As New SqlCommand("SELECT * FROM [CAY]", connection)
        Dim adapter_cay As New SqlDataAdapter(command_cay)
        Dim table_cay As New DataTable()
        adapter_cay.Fill(table_cay)
        cbx_TenCay.DataSource = table_cay
        cbx_TenCay.DisplayMember = "TENCAY"
        cbx_TenCay.ValueMember = "ID_CAY"

    End Sub

    Private Sub btn_ThemDV_Click(sender As Object, e As EventArgs) Handles btn_ThemDV.Click
        For i As Integer = DataGridView2.Rows.Count() - 1 To 0 Step -1
            Dim Insert As String
            Insert = DataGridView2.Rows(i).Cells(0).Value
            If Insert <> "" Then
                Dim updateQuery As String = "INSERT INTO [LICHCT]
                                                ([ID_LICHCT]
                                                ,[ID_LICHCHAMSOC]
                                                ,[ID_CHAMSOC])
                                                VALUES
                                                ('" & tbx_IDLichCT.Text & "',
                                                 '" & tbx_IDLich.Text & "',
                                                 '" & tbx_IDCScay.Text & "')"
                ExcuteQuery(updateQuery)
                MessageBox.Show("Add calender care Successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

                'set ID auto
                Dim result As Result
                Dim nextID = "1"
                result = lichCTBus.getNextID(nextID)
                If (result.FlagResult = False) Then
                    MessageBox.Show("Get the autocomplete code detailed care schedule failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    System.Console.WriteLine(result.SystemMessage)
                    Return
                End If
                tbx_IDLichCT.Text = nextID

                'Xoa dong da them tranh trung lap khoa chinh
                Dim row As DataGridViewRow
                row = DataGridView2.Rows(i)
                DataGridView2.Rows.Remove(row)
            End If
        Next
    End Sub

    Private Sub btn_Next1_Click(sender As Object, e As EventArgs) Handles btn_Next1.Click
        lichCTBus = New LichChamSoc_CTBUS()
        'set ID auto
        Dim result As Result
        Dim nextID = "1"
        result = lichCTBus.getNextID(nextID)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code detailed care schedule failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDLichCT.Text = nextID

        FiterData2("")
    End Sub

    Private Sub cbx_LichCS_TextChanged(sender As Object, e As EventArgs)
        cbx_TenCay.Enabled = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim frm As FrmThemLichNew = New FrmThemLichNew()
        frm.Show()
    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        Dim frm As FrmUpdateLichCS = New FrmUpdateLichCS()
        frm.Show()
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        index = e.RowIndex
        Dim SelectRow As DataGridViewRow
        If (index >= 0) Then
            SelectRow = DataGridView1.Rows(index)
            tbx_IDLich.Text = SelectRow.Cells(0).Value.ToString()
            dtp_TGLich.Value = Convert.ToDateTime(SelectRow.Cells(1).Value.ToString())
        End If
    End Sub

    Private Sub DataGridView2_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView2.CellMouseClick
        index = e.RowIndex
        Dim SelectRow As DataGridViewRow
        If (index >= 0) Then
            SelectRow = DataGridView2.Rows(index)
            tbx_IDCScay.Text = SelectRow.Cells(0).Value.ToString()
        End If
    End Sub
End Class