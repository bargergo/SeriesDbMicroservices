import React, { Component } from "react";
import {
  ISeriesRatingInfo,
  ISeriesRatingsClient,
  SeriesRatingsClient,
} from "../typings/RatingsClients";
import Rating from "./Rating";

interface IState {
  ratings: ISeriesRatingInfo[];
  loading: boolean;
}

interface IProps {
  client: ISeriesRatingsClient;
}

export default class RatingsContainer extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      ratings: [],
      loading: true,
    };
  }

  static defaultProps = {
    client: new SeriesRatingsClient(),
  };

  async componentDidMount() {
    const client: ISeriesRatingsClient = this.props.client;
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
