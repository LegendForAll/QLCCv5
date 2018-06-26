Imports QLCC_BUS
Imports QLCC_DTO
Imports Utility

Public Class FrmThemViTri
    Private VitriBus As ViTriCayBUS
    Private Sub ViTriCay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        VitriBus = New ViTriCayBUS()

        'set ID auto
        Dim result As Result
        Dim nextID = "1"
        result = VitriBus.getNextID(nextID)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code location failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDVitri.Text = nextID
    End Sub

    Private Sub btn_ThemVT_Click(sender As Object, e As EventArgs) Handles btn_ThemVT.Click
        Dim vitri As ViTriCayDTO
        vitri = New ViTriCayDTO()

        '1. Mapping data from GUI control
        vitri.Ma_VT = tbx_IDVitri.Text
        vitri.Ten_VT = tbx_TenViTri.Text

        '2. Business .....
        If (VitriBus.isValidName(vitri) = False) Then
            MessageBox.Show("The location name is incorrect. Recheck", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            tbx_TenViTri.Focus()
            Return
        End If
        '3. Insert to DB
        Dim result As Result
        result = VitriBus.insert(vitri)
        If (result.FlagResult = True) Then
            MessageBox.Show("More successful placement.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            tbx_TenViTri.Text = String.Empty

            'set ID auto
            Dim nextID = "1"
            result = VitriBus.getNextID(nextID)
            If (result.FlagResult = False) Then
                MessageBox.Show("Get the autocomplete code location failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                System.Console.WriteLine(result.SystemMessage)
                Return
            End If
            tbx_IDVitri.Text = nextID
        Else
            MessageBox.Show("Additional placement failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
        End If
    End Sub

    Private Sub btn_huy_Click(sender As Object, e As EventArgs) Handles btn_huy.Click
        Me.Close()
    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        Dim frm As FrmUpdateViTri = New FrmUpdateViTri()
        frm.ShowDialog()
    End Sub
End Class