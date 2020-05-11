import React, { Component } from "react";
import {
  ISeriesRatingInfo,
  SeriesRatingsClient,
} from "../typings/RatingsClients";
import Rating from "./Rating";

export default class RatingsContainer extends Component {
  state: { ratings: ISeriesRatingInfo[] } = {
    ratings: [],
  };

  async componentDidMount() {
    const client: SeriesRatingsClient = new SeriesRatingsClient();
    try {
      const response = await client.getSeriesRatings(undefined, undefined);
      this.setState({ ratings: response });
    } catch (err) {
      alert(err);
    }
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
