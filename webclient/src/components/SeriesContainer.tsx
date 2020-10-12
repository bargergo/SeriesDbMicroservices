import React, { Component } from "react";
import { Container, Row } from "react-bootstrap";
import { ISeriesClient, ISeriesInfo, SeriesClient } from "../typings/SeriesAndEpisodesClients";
import Series from "./Series";
import { Link } from "react-router-dom";

interface IState {
  series: ISeriesInfo[];
  loading: boolean;
}

interface IProps {
    client: ISeriesClient;
}

export default class SeriesContainer extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      series: [],
      loading: true,
    };
  }

  static defaultProps = {
      client: new SeriesClient()
  };

  async componentDidMount() {
    const client: ISeriesClient = this.props.client;
    try {
      const response = await client.getAllSeries();
      this.setState({ series: response, loading: false });
    } catch (err) {
      alert(err);
    }
  }

  render() {
    return (
      <>
        <h1>Series</h1>
        <Link to={"series/new"}>Add</Link>
        {this.state.loading ? (
          <p>
            <em>Loading...</em>
          </p>
        ) : (
          <Container>
            {this.state.series.map((s) => (
              <Row>
                <Series key={s.id} data={s} />
              </Row>
            ))}
          </Container>
        )}
      </>
    );
  }
}
