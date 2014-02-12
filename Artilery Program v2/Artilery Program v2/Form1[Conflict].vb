Public Class Form1
    Dim deg As Integer
    Dim map(10, 8) As Point
    Dim rand As New Random
    Dim drawExplosion As Boolean
    Dim panzer As Point = New Point(rand.Next(0, 11), rand.Next(0, 5))
    Dim shoot As Boolean
    Dim shot As Point
    Dim first As Boolean = True
    Function convertToRad(i As Double)
        Return i * (Math.PI / 180)
    End Function
    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        If shoot = True Then
            e.Graphics.DrawImage(My.Resources.explosion, shot.X * 75, shot.Y * 50, 75, 50)
            shoot = False
        End If
        For i = 0 To 10
            e.Graphics.DrawImage(My.Resources.barbed, i * 75, 250, 75, 50)
        Next
        e.Graphics.DrawImage(My.Resources.cannon, 375, 400, 73, 48)
        e.Graphics.DrawImage(My.Resources.panzer, panzer.X * 75 + 1, panzer.Y * 50 + 1, 73, 48)
        For row = 0 To 10
            For column = 0 To 8
                e.Graphics.DrawRectangle(Pens.Black, row * 75, column * 50, 75, 50)
                map(row, column) = New Point(row - 5, 8 - column)
                e.Graphics.DrawString(row - 5 & ", " & 8 - column, SystemFonts.DefaultFont, Brushes.Black, row * 75, column * 50)
            Next
        Next
        If shot = panzer Then
            e.Graphics.DrawImage(My.Resources.explosion, shot.X * 75, shot.Y * 50, 75, 50)
            MessageBox.Show("Nice shot!")
            Form2.Show()
            Me.Close()
        End If
    End Sub
    Private Sub fire_Click(sender As Object, e As EventArgs) Handles fire.Click
        Dim angle As Double
        Dim force As Integer
        Dim x As Integer
        Dim y As Integer
        Try
            angle = TextBox1.Text
            force = TextBox2.Text
        Catch ex As Exception
            MessageBox.Show("Please use only numbers when inputing your angle or force")
            TextBox1.Text = 90
            TextBox2.Text = 0
        End Try
        If angle <= 180 And angle >= 0 Then
            angle = convertToRad(angle)
            x = Math.Round((force * (Math.Cos(angle))))
            y = Math.Round(force * (Math.Sin(angle)))
            shot = New Point(x + 5, 8 - y)
            If shot.X < 0 Or shot.X > 10 Then
                MessageBox.Show("Your shot is out of bounds, please try using a different force")
                Me.Refresh()
            ElseIf shot.Y < 0 Or shot.Y > 8 Then
                MessageBox.Show("Your shot is out of bounds, please try using a different force")
                Me.Refresh()
            Else
                shoot = True
                Me.Refresh()
            End If
        ElseIf angle > 180 Then
            MessageBox.Show("Degree value is too large. Please insert a number within the range of 0 and 180")
        ElseIf angle < 0 Then
            MessageBox.Show("Degree value is too small. Please insert a number within the range of 0 and 180")
        End If
    End Sub
    Function convMapToGrid(ByVal pos As Point)
        pos.X += 5
        pos.Y = 8 - pos.Y
        Return pos
    End Function
    Function movePanz()
        Dim moveDir As Integer
        Dim moveDis As Integer
        Dim temp As String
        moveDir = rand.Next(0, 2)
        temp = rand.Next(0, 2)
        If temp = 0 Then
            moveDis = -1
        Else
            moveDis = 1
        End If
        If panzer.X < 0 Then
            If panzer.Y < 0 Then
                If moveDir = 0 Then
                    panzer.X += moveDis
                Else
                    panzer.Y += moveDis
                End If
            End If
        Else
            MessageBox.Show("Please work")
        End If

    End Function
End Class
