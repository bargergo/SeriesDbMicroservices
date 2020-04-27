import React, { Component } from 'react';
import { SeriesClient, SeriesInfo } from './typings/SeriesAndEpisodesClients';
import logo from './logo.svg';
import './App.css';
import Series from './Series';


class App extends Component {

  state: { series: SeriesInfo[] } = {
    series: []
  }

  componentDidMount() {
    const client: SeriesClient = new SeriesClient(".");
    client.getAllSeries()
      .then(response => 
        this.setState({series: response})
      )
      .catch(error => 
        console.log(error)
      );
  }

  render() {
    console.log(this.state);
    return (
      <div className="App">
        <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <p>
            Edit <code>src/App.tsx</code> and save to reload.
          </p>
          <div>{this.state.series.map(series => <Series data={series} key={series.id} />)}</div>
          <a
            className="App-link"
            href="https://reactjs.org"
            target="_blank"
            rel="noopener noreferrer"
          >
            Learn React
          </a>
        
        </header>
      </div>
    );
  }
  
}

export default App;
