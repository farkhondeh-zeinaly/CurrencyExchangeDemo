import logo from './logo.svg';
import './App.css';
import ApiPage from './ApiPage';

function App() {
  return (
    <div className="App">
      <header className="">
        <img src={logo} className="App-logo" alt="logo" />
        
        </header>
        <p>Fetching Data:</p>
        <div>
          <ApiPage/>
        </div>
    </div>
  );
}

export default App;
