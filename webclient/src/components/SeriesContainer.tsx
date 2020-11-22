import React, { Component } from "react";
import { Container, Row } from "react-bootstrap";
import { Link, RouteComponentProps } from "react-router-dom";
import ClientsContext from "../ClientsContext";
import { ISeriesClient, ISeriesInfo } from "../typings/SeriesAndEpisodesClients";
import Series from "./Series";

interface IState {
  series: ISeriesInfo[];
  loading: boolean;
}

interface IProps {
}

export default class SeriesContainer extends Component<RouteComponentProps<IProps>, IState> {
  constructor(props: RouteComponentProps<IProps>) {
    super(props);
    this.state = {
      series: [],
      loading: true,
    };
  }

  static contextType = ClientsContext;

  async componentDidMount() {
    const client: ISeriesClient = this.context.seriesClient;
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
                <Series key={s.id} data={s} path={this.props.match.path}/>
              </Row>
            ))}
          </Container>
        )}
      </>
    );
  }
}
