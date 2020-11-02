import React from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import "./App.css";
import ClientsContext from "./ClientsContext";
import About from "./components/About";
import Header from "./components/Header";
import Home from "./components/Home";
import RatingsContainer from "./components/RatingsContainer";
import SeriesContainer from "./components/SeriesContainer";
import SeriesDetailComponent from "./components/SeriesDetailComponent";
import SeriesDetailForm from "./components/SeriesDetailForm";
import SignInComponent from "./components/SignInComponent";
import { EpisodeRatingsClient, SeriesRatingsClient } from "./typings/RatingsClients";
import { ImageClient, SeriesClient } from "./typings/SeriesAndEpisodesClients";

const App = () => (
  <ClientsContext.Provider value={{
      seriesClient: new SeriesClient(),
      imageClient: new ImageClient(),
      seriesRatingClient: new SeriesRatingsClient(),
      episodeRatingClient: new EpisodeRatingsClient()
  }}>
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
          <Route exact path="/Signin" component={SignInComponent} />
          <Route path="/about" component={About} />
        </Switch>
      </div>
    </BrowserRouter>
  </ClientsContext.Provider>
);

export default App;
