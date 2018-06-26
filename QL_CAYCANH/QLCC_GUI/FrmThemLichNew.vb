Imports QLCC_BUS
Imports QLCC_DTO
Imports Utility
Imports System.Data.SqlClient
Imports System.Data.DataTable
Imports System.Configuration

Public Class FrmThemLichNew
    Private lichBus As LichChamSocBUS
    'Dim connection As New SqlConnection("Data Source=DESKTOP-SNJJV8M;Initial Catalog=QLCC;Integrated Security=True")
    ' Read ConnectionString value from App.config file
    Dim connection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))

    Private Sub FrmThemLichNew_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lichBus = New LichChamSocBUS()

        'set ID auto
        Dim result As Result
        Dim nextID = "1"
        result = lichBus.getNextID(nextID)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code calendar care failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDLich.Text = nextID
    End Sub

    Private Sub btn_Insert_Click(sender As Object, e As EventArgs) Handles btn_Insert.Click
        Dim lichCS As LichChamSocDTO
        lichCS = New LichChamSocDTO()

        '1. Mapping data from GUI control
        lichCS.MS_LichCS = tbx_IDLich.Text
        lichCS.TG_ChamSoc = dtp_TGLich.Value

        '2. Business .....

        '3. Insert to DB
        Dim result As Result
        result = lichBus.insert(lichCS)

        If (result.FlagResult = True) Then
            MessageBox.Show("Add calendar care successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

            'set ID auto
            Dim nextID = "1"
            result = lichBus.getNextID(nextID)
            If (result.FlagResult = False) Then
                MessageBox.Show("Get the autocomplete code calendar care failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                System.Console.WriteLine(result.SystemMessage)
                Return
            End If
            tbx_IDLich.Text = nextID

        Else
            MessageBox.Show("Add calendar care unsuccessful.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
        End If
    End Sub

    Private Sub btn_huy_Click(sender As Object, e As EventArgs) Handles btn_huy.Click
        Me.Close()
    End Sub
End Class