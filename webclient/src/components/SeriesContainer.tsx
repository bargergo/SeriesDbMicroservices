import React, { Component } from "react";
import { Container, Row } from "react-bootstrap";
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
      <>
        <h1>Series</h1>
        <Container>
          {this.state.series.map((s) => (
            <Row>
              <Series key={s.id} data={s} />
            </Row>
          ))}
        </Container>
      </>
    );
  }
}
