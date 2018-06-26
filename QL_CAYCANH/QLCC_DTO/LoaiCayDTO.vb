Public Class LoaiCayDTO
    Private iMaLC As String
    Private strTenLC As String

    Public Sub New()

    End Sub
    Public Sub New(iMaLC As String, strTenLC As String)
        Me.iMaLC = iMaLC
        Me.strTenLC = strTenLC
    End Sub
    Property MS_LoaiCay() As String
        Get
            Return iMaLC
        End Get
        Set(ByVal Value As String)
            iMaLC = Value
        End Set
    End Property
    Property TenLoai_Cay() As String
        Get
            Return strTenLC
        End Get
        Set(ByVal Value As String)
            strTenLC = Value
        End Set
    End Property
End Class
