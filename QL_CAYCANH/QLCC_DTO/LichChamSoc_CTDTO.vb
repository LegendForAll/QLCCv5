Public Class LichChamSoc_CTDTO
    Private iMaLichCS_CT As String
    Private iMaLich_CS As String
    Private iMaCS As String

    Public Sub New()

    End Sub
    Public Sub New(iMaLichCS_CT As String, iMaLich_CS As String, iMaCS As String)
        Me.iMaLichCS_CT = iMaLichCS_CT
        Me.iMaLich_CS = iMaLich_CS
        Me.iMaCS = iMaCS
    End Sub
    Property MS_LichCSCT() As String
        Get
            Return iMaLichCS_CT
        End Get
        Set(ByVal Value As String)
            iMaLichCS_CT = Value
        End Set
    End Property
    Property MS_LichCS() As String
        Get
            Return iMaLich_CS
        End Get
        Set(ByVal Value As String)
            iMaLich_CS = Value
        End Set
    End Property
    Property MS_ChamSoc() As String
        Get
            Return iMaCS
        End Get
        Set(ByVal Value As String)
            iMaCS = Value
        End Set
    End Property
End Class
