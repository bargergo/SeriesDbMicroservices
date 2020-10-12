import React from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import "./App.css";
import About from "./components/About";
import Header from "./components/Header";
import Home from "./components/Home";
import RatingsContainer from "./components/RatingsContainer";
import SeriesContainer from "./components/SeriesContainer";
import SeriesDetailComponent from "./components/SeriesDetailComponent";
import SeriesDetailForm from "./components/SeriesDetailForm";

const App = () => (
  <BrowserRouter>
    <Header />
    <div className="container">
      <Switch>
        <Route exact path="/" component={Home} />
        <Route exact path="/Series" component={SeriesContainer} />
        <Route exact path="/Series/new" component={SeriesDetailForm} />
        <Route exact path="/Series/:id/edit" component={SeriesDetailForm} />
        <Route exact path="/Series/:id" component={SeriesDetailComponent} />
        <Route exact path="/Ratings" component={RatingsContainer} />
        <Route path="/about" component={About} />
      </Switch>
    </div>
  </BrowserRouter>
);

export default App;
