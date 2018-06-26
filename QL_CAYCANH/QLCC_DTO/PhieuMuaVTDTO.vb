Public Class PhieuMuaVTDTO
    '[ID_PHIEUMUA_VT] [nchar](10) Not NULL,
    '[NGAYMUAVT] [datetime2](7) Not NULL,
    Private strMaPhieu As String
    Private dateNgayMua As DateTime

    Public Sub New()

    End Sub
    Public Sub New(strMaPhieu As String, dateNgayMua As DateTime)
        Me.strMaPhieu = strMaPhieu
        Me.dateNgayMua = dateNgayMua
    End Sub
    Property MS_Phieu() As String
        Get
            Return strMaPhieu
        End Get
        Set(ByVal Value As String)
            strMaPhieu = Value
        End Set
    End Property
    Property TG_Mua() As DateTime
        Get
            Return dateNgayMua
        End Get
        Set(ByVal Value As DateTime)
            dateNgayMua = Value
        End Set
    End Property
End Class
