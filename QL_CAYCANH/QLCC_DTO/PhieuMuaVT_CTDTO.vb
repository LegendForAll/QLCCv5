Public Class PhieuMuaVT_CTDTO
    '[ID_PHIEUMUA_CT] [nchar](10) Not NULL,
    '[ID_PHIEUMUA_VT] [nchar](10) Not NULL,
    '[DIACHIMUA] [nvarchar](50) Not NULL,
    '[SOLUONGMUA] [float] Not NULL,
    '[SOTIEN] [money] Not NULL,
    '[ID_VATTU] [int] Not NULL,
    Private strMaPhieu_CT As String
    Private strMaPhieu As String
    Private iMaVatTu As String
    Private strDiaChi As String
    Private iSoLuong_Mua As Integer
    Private dSoTien As Integer


    Public Sub New()

    End Sub
    Public Sub New(strMaPhieu_CT As String, strMaPhieu As String, iMaVatTu As String, strDiaChi As String, iSoLuong_Mua As Integer, dSoTien As Integer)
        Me.strMaPhieu_CT = strMaPhieu_CT
        Me.strMaPhieu = strMaPhieu
        Me.iMaVatTu = iMaVatTu
        Me.strDiaChi = strDiaChi
        Me.iSoLuong_Mua = iSoLuong_Mua
        Me.dSoTien = dSoTien
    End Sub
    Property MS_CTPhieu() As String
        Get
            Return strMaPhieu_CT
        End Get
        Set(ByVal Value As String)
            strMaPhieu_CT = Value
        End Set
    End Property
    Property MS_Phieu() As String
        Get
            Return strMaPhieu
        End Get
        Set(ByVal Value As String)
            strMaPhieu = Value
        End Set
    End Property
    Property MS_VatTu() As String
        Get
            Return iMaVatTu
        End Get
        Set(ByVal Value As String)
            iMaVatTu = Value
        End Set
    End Property
    Property DiaChi() As String
        Get
            Return strDiaChi
        End Get
        Set(ByVal Value As String)
            strDiaChi = Value
        End Set
    End Property
    Property SoLuongMua() As Integer
        Get
            Return iSoLuong_Mua
        End Get
        Set(ByVal Value As Integer)
            iSoLuong_Mua = Value
        End Set
    End Property
    Property SoTien() As Integer
        Get
            Return dSoTien
        End Get
        Set(ByVal Value As Integer)
            dSoTien = Value
        End Set
    End Property

End Class
