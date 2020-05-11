import React, { Component } from "react";
import { ISeriesInfo, SeriesClient } from "../typings/SeriesAndEpisodesClients";
import Series from "./Series";

export default class SeriesContainer extends Component {
  state: { series: ISeriesInfo[] } = {
    series: [],
  };

  async componentDidMount() {
    const client: SeriesClient = new SeriesClient();
    try {
      const response = await client.getAllSeries();
      this.setState({ series: response });
    } catch (err) {
      alert(err);
    }
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
