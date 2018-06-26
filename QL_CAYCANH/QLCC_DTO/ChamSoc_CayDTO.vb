Public Class ChamSoc_CayDTO
    Private iMaCS As String
    Private strMSCay As String
    Private iMaVatTu As String
    Private iSoLuongCS As Integer
    Private dateTGChamSoc As DateTime
    Private strTinhTrang As String
    Private strGhiChu As String

    Public Sub New()

    End Sub
    Public Sub New(iMaCS As String, strMSCay As String, iMaVatTu As String, iSoLuongCS As Integer, dateTGChamSoc As DateTime, strTinhTrang As String, strGhiChu As String)
        Me.iMaCS = iMaCS
        Me.strMSCay = strMSCay
        Me.iMaVatTu = iMaVatTu
        Me.iSoLuongCS = iSoLuongCS
        Me.dateTGChamSoc = dateTGChamSoc
        Me.strTinhTrang = strTinhTrang
        Me.strGhiChu = strGhiChu
    End Sub
    Property MS_ChamSoc() As String
        Get
            Return iMaCS
        End Get
        Set(ByVal Value As String)
            iMaCS = Value
        End Set
    End Property
    Property MS_Cay() As String
        Get
            Return strMSCay
        End Get
        Set(ByVal Value As String)
            strMSCay = Value
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
    Property SoLuong_CS() As Integer
        Get
            Return iSoLuongCS
        End Get
        Set(ByVal Value As Integer)
            iSoLuongCS = Value
        End Set
    End Property
    Property TG_ChamSoc() As DateTime
        Get
            Return dateTGChamSoc
        End Get
        Set(ByVal Value As DateTime)
            dateTGChamSoc = Value
        End Set
    End Property
    Property TinhTrang_Cay() As String
        Get
            Return strTinhTrang
        End Get
        Set(ByVal Value As String)
            strTinhTrang = Value
        End Set
    End Property
    Property GhiChu_Cay() As String
        Get
            Return strGhiChu
        End Get
        Set(ByVal Value As String)
            strGhiChu = Value
        End Set
    End Property
End Class
