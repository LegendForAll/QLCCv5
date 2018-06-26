Imports QLCC_BUS
Imports QLCC_DTO
Imports Utility
Imports System.Data.SqlClient
Imports System.Configuration

Public Class Frm_ThemVatTu
    Private VatTuBus As VaTuBUS
    Private donviBus As DonViBUS
    'Dim connection As New SqlConnection("Data Source=DESKTOP-SNJJV8M;Initial Catalog=QLCC;Integrated Security=True")
    ' Read ConnectionString value from App.config file
    Dim connection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))

    Private Sub Frm_ThemVatTu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        VatTuBus = New VaTuBUS()
        donviBus = New DonViBUS()

        'set ID auto
        Dim result As Result
        Dim nextID = "1"
        result = VatTuBus.getNextID(nextID)
        If (result.FlagResult = False) Then
            MessageBox.Show("Get the autocomplete code supplies failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbx_IDVatTu.Text = nextID

        ' Load loai cay list

        Dim command As New SqlCommand("SELECT * FROM [DONVI]", connection)
        Dim adapter As New SqlDataAdapter(command)
        Dim table As New DataTable()
        adapter.Fill(table)
        cbx_Donvi.DataSource = table
        cbx_Donvi.DisplayMember = "TEN_DONVI"
        cbx_Donvi.ValueMember = "ID_DONVI"
    End Sub

    Private Sub btn_ThemVT_Click(sender As Object, e As EventArgs) Handles btn_ThemVT.Click
        Dim vatt As VatTuDTO
        vatt = New VatTuDTO()

        '1. Mapping data from GUI control
        vatt.MS_VatTu = tbx_IDVatTu.Text
        vatt.Ten_VatTu = tbx_TenVatTu.Text
        vatt.MS_DonVi = Convert.ToInt32(cbx_Donvi.SelectedValue)
        '2. Business .....
        If (VatTuBus.isValidName(vatt) = False) Then
            MessageBox.Show("Tên vat tu không đúng. Vui lòng kiểm tra lại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            tbx_TenVatTu.Focus()
            Return
        End If
        '3. Insert to DB
        Dim result As Result
        result = VatTuBus.insert(vatt)
        If (result.FlagResult = True) Then
            MessageBox.Show("Thêm vat tu thành công.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            tbx_TenVatTu.Text = String.Empty

            'set ID auto
            Dim nextID = "1"
            result = VatTuBus.getNextID(nextID)
            If (result.FlagResult = False) Then
                MessageBox.Show("Get the autocomplete code supplies failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                System.Console.WriteLine(result.SystemMessage)
                Return
            End If
            tbx_IDVatTu.Text = nextID

        Else
            MessageBox.Show("Thêm vat tu không thành công.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
        End If
    End Sub

    Private Sub btn_huy_Click(sender As Object, e As EventArgs) Handles btn_huy.Click
        Me.Close()
    End Sub
End Class