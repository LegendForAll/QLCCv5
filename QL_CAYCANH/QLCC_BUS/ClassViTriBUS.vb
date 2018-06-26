Imports QLCC_DAL
Imports QLCC_DTO
Imports Utility

Public Class ClassViTriBUS
    Private viTriDAL As ClassVitriDAL
    Public Sub New()
        viTriDAL = New ClassVitriDAL()
    End Sub
    Public Function insert(ViTri As ViTriCayDTO) As Boolean
        '1. verify data here!!

        '2. insert to DB
        Return viTriDAL.insert(ViTri)
    End Function

    Public Function update(ViTri As ViTriCayDTO) As Boolean
        '1. verify data here!!

        '2. insert to DB
        Return viTriDAL.update(ViTri)
    End Function
End Class
