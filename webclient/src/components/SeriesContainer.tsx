import React, { Component } from "react";
import { Container, Row, Button } from "react-bootstrap";
import { ISeriesInfo, SeriesClient } from "../typings/SeriesAndEpisodesClients";
import Series from "./Series";
import { Link } from "react-router-dom";

interface IState {
  series: ISeriesInfo[];
  loading: boolean;
}

interface IProps {}

export default class SeriesContainer extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      series: [],
      loading: true,
    };
  }

  componentDidMount() {
    this.loadData();
  }

  async loadData() {
    this.setState({ loading: true });
    const client: SeriesClient = new SeriesClient();
    try {
      const response = await client.getAllSeries();
      this.setState({ series: response, loading: false });
    } catch (err) {
      alert(err);
    }
  }

  async handleDelete(id: string) {
    const client: SeriesClient = new SeriesClient();
    try {
      await client.deleteSeries(id);
    } catch (err) {
      alert(err);
    }
    this.loadData();
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
                <Button variant="link" onClick={() => this.handleDelete(s.id)}>
                  Delete
                </Button>
              </Row>
            ))}
          </Container>
        )}
      </>
    );
  }
}
