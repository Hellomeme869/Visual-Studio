Public Class Form1
    Dim map(10, 8) As Point
    Dim rand As New Random
    Dim panzer As Point = New Point(rand.Next(0, 11), rand.Next(0, 5))
    Dim shoot As Boolean
    Dim shot As Point
    Dim ammo As Integer = 5
    Dim close1 As Boolean
    Function convertToRad(i As Double)
        Return i * (Math.PI / 180)
    End Function
    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        If close1 = True Then
            closeApp()
        End If
        For row = 0 To 10
            For column = 0 To 8
                e.Graphics.DrawRectangle(Pens.Black, row * 75, column * 50, 75, 50)
                map(row, column) = New Point(row - 5, 8 - column)
                'e.Graphics.DrawString(row - 5 & ", " & 8 - column, SystemFonts.DefaultFont, Brushes.Black, row * 75, column * 50)
            Next
        Next
        For i = 0 To 10
            e.Graphics.DrawImage(My.Resources.barbed, i * 75, 250, 75, 50)
        Next
        e.Graphics.DrawImage(My.Resources.cannon, 375, 400, 73, 48)
        If shoot = True Then
            If ammo > 0 Then
                e.Graphics.DrawImage(My.Resources.explosion, shot.X * 75, shot.Y * 50, 75, 50)
                shoot = False
                If shot = panzer Then
                    e.Graphics.DrawImage(My.Resources.explosion, shot.X * 75, shot.Y * 50, 75, 50)
                    MessageBox.Show("Nice shot!")
                    closeApp()
                Else
                    ammo -= 1
                    Label3.Text = "Ammo: " & ammo
                    movePanz()
                End If
            End If
        End If
        If ammo = 0 Then
            ammo = 1
            Dim thread1 As New Threading.Thread(Sub() killPlayer())
            thread1.Start()
        End If
        Try
            e.Graphics.DrawImage(My.Resources.panzer, panzer.X * 75 + 1, panzer.Y * 50 + 1, 73, 48)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub fire_Click(sender As Object, e As EventArgs) Handles fire.Click
        Dim angle As Double
        Dim force As Double
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
            If shot.X < 0 Or shot.X > 10 Or shot.Y < 0 Or shot.Y > 8 Then
                MessageBox.Show("Your shot is out of bounds, please try using a different force")
                Me.Refresh()
            Else
                shoot = True
                Me.Refresh()
            End If
        End If
        If angle > 180 Then
            MessageBox.Show("Degree value is too large. Please insert a number within the range of 0 and 180")
        End If
        If angle < 0 Then
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
        moveDir = rand.Next(0, 2)
        moveDis = rand.Next(0, 2)
        If moveDis = 0 Then
            moveDis = -1
        Else
            moveDis = 1
        End If
        Select Case moveDir
            Case 0
                Select Case panzer.Y
                    Case 0
                        panzer.Y += 1
                    Case 4
                        panzer.Y += -1
                    Case Else
                        panzer.Y += moveDis
                End Select
            Case 1
                Select Case panzer.X
                    Case 0
                        panzer.X += 1
                    Case 10
                        panzer.X += -1
                    Case Else
                        panzer.X += moveDis
                End Select
        End Select
        Return Nothing
    End Function
    Sub killPlayer()
        If Me.InvokeRequired Then
            Me.Invoke(Sub() killPlayer())
        Else
            MessageBox.Show("You have run out of ammo")
            While close1 = False
                Select Case panzer.X
                    Case 0 To 4
                        For i = panzer.X To 5
                            panzer.X = i
                            Threading.Thread.Sleep(300)
                            Me.Refresh()
                        Next
                    Case 5
                        For i = panzer.Y To 8
                            panzer.Y = i
                            Threading.Thread.Sleep(300)
                            Me.Refresh()
                        Next
                        If panzer.Y = 8 Then
                            Threading.Thread.Sleep(300)
                            close1 = True
                            Me.Refresh()
                        End If
                    Case 6 To 10
                        For i = panzer.X To 5 Step -1
                            panzer.X = i
                            Threading.Thread.Sleep(300)
                            Me.Refresh()
                        Next
                End Select
            End While
        End If
    End Sub
    Sub closeApp()
        Form2.Show()
        Me.Close()
    End Sub
End Class
