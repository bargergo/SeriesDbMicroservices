import React, { Component } from "react";
import {
  ISeriesRatingInfo,
  SeriesRatingsClient,
} from "../typings/RatingsClients";
import Rating from "./Rating";

class RatingsContainer extends Component {
  state: { ratings: ISeriesRatingInfo[] } = {
    ratings: [],
  };

  componentDidMount() {
    const client: SeriesRatingsClient = new SeriesRatingsClient();
    client
      .getSeriesRatings(undefined, undefined)
      .then((response) => this.setState({ ratings: response }))
      .catch((error) => console.log(error));
  }

  render() {
    return (
      <div>
        <h2>Ratings</h2>
        {this.state.ratings.map((s) => (
          <Rating key={s.id} data={s} />
        ))}
      </div>
    );
  }
}

export default RatingsContainer;
