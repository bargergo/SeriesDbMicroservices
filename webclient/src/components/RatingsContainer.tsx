import React, { Component } from "react";
import {
  ISeriesRatingInfo,
  SeriesRatingsClient,
} from "../typings/RatingsClients";
import Rating from "./Rating";

interface IState {
  ratings: ISeriesRatingInfo[];
  loading: boolean;
}

interface IProps {}

export default class RatingsContainer extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      ratings: [],
      loading: true,
    };
  }

  async componentDidMount() {
    const client: SeriesRatingsClient = new SeriesRatingsClient();
    try {
      const response = await client.getSeriesRatings(undefined, undefined);
      this.setState({ ratings: response, loading: false });
    } catch (err) {
      alert(err);
    }
  }

  render() {
    return (
      <>
        <h2>Ratings</h2>
        {this.state.loading ? (
          <p>
            <em>Loading...</em>
          </p>
        ) : (
          this.state.ratings.map((s) => <Rating key={s.id} data={s} />)
        )}
      </>
    );
  }
}
