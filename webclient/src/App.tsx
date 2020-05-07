import React from "react";
import { BrowserRouter, Route } from "react-router-dom";
import "./App.css";
import About from "./components/About";
import Header from "./components/Header";
import Home from "./components/Home";
import RatingsContainer from "./components/RatingsContainer";
import SeriesContainer from "./components/SeriesContainer";

const App = () => (
  <BrowserRouter>
    <div className="container">
      <Header />
      <Route exact path="/" component={Home} />
      <Route exact path="/series" component={SeriesContainer} />
      <Route exact path="/ratings" component={RatingsContainer} />
      <Route path="/about" component={About} />
    </div>
  </BrowserRouter>
);

export default App;
