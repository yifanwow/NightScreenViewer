document.addEventListener('DOMContentLoaded', () => {
    const settingsApp = document.getElementById('settingsApp');
  
    settingsApp.innerHTML = `
      <div class="container">
        <h1>Settings Page</h1>
        <p>updating coming soon<p>
        <button class="button" id="backButton">Back</button>
      </div>
    `;
  
    document.getElementById('backButton').addEventListener('click', () => {
      window.electron.goBack();
    });
  });
  