const { Menu, shell, app } = require('electron');

function createMenu(mainWindow) {
  const menuTemplate = [
    {
      label: 'Setting',
      click: () => {
        mainWindow.loadFile('settings.html');
      }
    },
    {
      label: 'Document',
      click: async () => {
        await shell.openExternal('https://github.com/yifanwow/NightScreenViewer');
      }
    },
    { type: 'separator' },
    {
      label: 'Exit',
      click: () => {
        app.isQuiting = true;
        app.quit();
      }
    }
  ];

  const menu = Menu.buildFromTemplate(menuTemplate);
  Menu.setApplicationMenu(menu);
}

module.exports = { createMenu };
