import React, { Component } from "react";
import { ISeriesInfo, SeriesClient } from "../typings/SeriesAndEpisodesClients";
import Series from "./Series";

class SeriesContainer extends Component {
  state: { series: ISeriesInfo[] } = {
    series: [],
  };

  componentDidMount() {
    const client: SeriesClient = new SeriesClient(".");
    client
      .getAllSeries()
      .then((response) => this.setState({ series: response }))
      .catch((error) => console.log(error));
  }

  render() {
    return (
      <div>
        <h2>Series</h2>
        {this.state.series.map((s) => (
          <Series key={s.id} data={s} />
        ))}
      </div>
    );
  }
}

export default SeriesContainer;
