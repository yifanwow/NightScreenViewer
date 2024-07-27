const { app, BrowserWindow, Menu, ipcMain, Tray, shell } = require('electron');
const path = require('path');
const { spawn } = require('child_process');
const { createMenu } = require('./menu');
const { createTray } = require('./tray');
const net = require('net');

let mainWindow;
let tray = null;
let client = null;

function createWindow() {
  mainWindow = new BrowserWindow({
    width: 700,
    height: 500,
    minWidth: 350,
    minHeight: 250,
    icon: path.join(__dirname, 'IMG/logo/logo.png'), // 设置窗口图标
    webPreferences: {
      preload: path.join(__dirname, 'preload.js'),
      nodeIntegration: false,
      contextIsolation: true
    }
  });

  mainWindow.loadFile('index.html');

  // 调用自定义的菜单创建函数
  createMenu(mainWindow);

  // 创建托盘图标
  tray = createTray(mainWindow);

  // 窗口关闭行为：最小化到托盘
  mainWindow.on('close', (event) => {
    if (!app.isQuiting) {
      event.preventDefault();
      mainWindow.hide();
    }
  });

  // 处理来自渲染进程的消息
  ipcMain.on('message', (event, message) => {
    console.log('Message from renderer:', message);
    if (client) {
      client.write(message + '\n');
    }
    event.reply('message-received');
  });

  ipcMain.on('go-back', () => {
    mainWindow.loadFile('index.html');
  });

  // 启动 C# 后端进程
  const backendPath = path.join(__dirname, '../backend/NightScreenViewerBackend');
  const backendProcess = spawn('dotnet', ['run', '--project', backendPath]);

  backendProcess.stdout.on('data', (data) => {
    console.log(`Backend stdout: ${data}`); // 输出原始数据
    const messageToSend = data.toString().trim();
    console.log(`Sending message to frontend: '${messageToSend}'`); // 明确显示发送的消息
    mainWindow.webContents.send('backend-message', messageToSend);
  });

  backendProcess.stderr.on('data', (data) => {
    console.error(`Backend error: ${data}`);
  });

  backendProcess.on('close', (code) => {
    console.log(`Backend process exited with code ${code}`);
  });

  // 连接到 C# 后端的命名管道
  const connectToPipe = () => {
    client = net.connect({ path: '\\\\.\\pipe\\NightScreenViewerPipe' }, () => {
      console.log('Connected to C# backend');
    });

    client.on('data', (data) => {
      console.log(`Received from C# backend: ${data}`);
      mainWindow.webContents.send('backend-message', data.toString());
    });

    client.on('error', (err) => {
      console.error('Named pipe client error:', err);
      setTimeout(connectToPipe, 1000); // 重试连接
    });
  };

  connectToPipe();
}

app.whenReady().then(createWindow);

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('activate', () => {
  if (BrowserWindow.getAllWindows().length === 0) {
    createWindow();
  }
});
