import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import {
  ISeriesDetail,
  SeriesClient,
} from "../typings/SeriesAndEpisodesClients";
import SeriesRatingForm from "./SeriesRatingForm";

type TParams = { id: string };

class SeriesDetail extends Component<
  RouteComponentProps<TParams>,
  ISeriesDetail
> {
  constructor(props: RouteComponentProps<TParams>) {
    super(props);
    this.state = {
      seasons: [],
      averageRating: -1,
      numberOfRatings: -1,
      id: "",
      title: "",
      description: "",
      firstAired: new Date(),
      lastUpdated: new Date(),
      imageId: "",
    };
  }

  async componentDidMount() {
    const client: SeriesClient = new SeriesClient();
    try {
      const response = await client.getSeries(this.props.match.params.id);
      this.setState(response);
    } catch (err) {
      alert(err);
    }
  }

  render() {
    return (
      <div>
        <h2>Series Detail</h2>
        {this.state.title}
        <img src={`/api/Images/${this.state.imageId}`} alt="cover"></img>
        {this.state.description}
        {this.state.averageRating}
        <SeriesRatingForm seriesId={this.state.id} />
      </div>
    );
  }
}

export default SeriesDetail;
