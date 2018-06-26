Imports QLCC_BUS
Imports QLCC_DTO
Imports Utility

Public Class FrmThemLoaiCay
    Private LoaiCayBus As LoaiCayBUS

    Private Sub LoaiCay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoaiCayBus = New LoaiCayBUS()

        'set ID auto
        Dim result As Result
        Dim nextID = "1"
        result = LoaiCayBus.getNextID(nextID)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code type's tree failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDLoaiCay.Text = nextID
    End Sub

    Private Sub btn_ThemLoai_Click(sender As Object, e As EventArgs) Handles btn_ThemLoai.Click
        Dim loaicay As LoaiCayDTO
        loaicay = New LoaiCayDTO()

        '1. Mapping data from GUI control
        loaicay.MS_LoaiCay = tbx_IDLoaiCay.Text
        loaicay.TenLoai_Cay = tbx_TenLoai.Text

        '2. Business .....
        If (LoaiCayBus.isValidName(loaicay) = False) Then
            MessageBox.Show("The type's tree name is incorrect. Recheck", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            tbx_TenLoai.Focus()
            Return
        End If
        '3. Insert to DB
        Dim result As Result
        result = LoaiCayBus.insert(loaicay)
        If (result.FlagResult = True) Then
            MessageBox.Show("Add type's tree is successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            tbx_TenLoai.Text = String.Empty

            'set ID auto
            Dim nextID = "1"
            result = LoaiCayBus.getNextID(nextID)
            If (result.FlagResult = False) Then
                MessageBox.Show("Get the autocomplete code type's tree failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                System.Console.WriteLine(result.SystemMessage)
                Return
            End If
            tbx_IDLoaiCay.Text = nextID

        Else
            MessageBox.Show("TAdd type's tree is unsuccessful.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
        End If
    End Sub

    Private Sub btn_huy_Click(sender As Object, e As EventArgs) Handles btn_huy.Click
        Me.Close()
    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        Dim frm As FrmUpdateLoaiCay = New FrmUpdateLoaiCay()
        frm.Show()
    End Sub
End Class