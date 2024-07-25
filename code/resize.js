function enforceAspectRatio(mainWindow) {
  const aspectRatio = 500 / 700;
  mainWindow.on('will-resize', (event, newBounds) => {
    if (newBounds.width / newBounds.height !== aspectRatio) {
      event.preventDefault();
      if (newBounds.width / newBounds.height > aspectRatio) {
        newBounds.width = Math.floor(newBounds.height * aspectRatio);
      } else {
        newBounds.height = Math.floor(newBounds.width / aspectRatio);
      }
      mainWindow.setBounds(newBounds);
    }
  });
}

module.exports = { enforceAspectRatio };
