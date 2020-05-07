import React from "react";
import { BrowserRouter, Route } from "react-router-dom";
import "./App.css";
import About from "./components/About";
import Header from "./components/Header";
import Home from "./components/Home";
import RatingsContainer from "./components/RatingsContainer";
import SeriesContainer from "./components/SeriesContainer";
import SeriesDetail from "./components/SeriesDetail";

const App = () => (
  <BrowserRouter>
    <div className="container">
      <Header />
      <Route exact path="/" component={Home} />
      <Route exact path="/Series" component={SeriesContainer} />
      <Route exact path="/Series/:id" component={SeriesDetail} />
      <Route exact path="/Ratings" component={RatingsContainer} />
      <Route path="/about" component={About} />
    </div>
  </BrowserRouter>
);

export default App;
