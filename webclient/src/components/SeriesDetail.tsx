import React, { Component } from "react";
import { RouteComponentProps, Link } from "react-router-dom";
import {
  ISeriesClient,
  ISeriesDetail,
  SeriesClient,
} from "../typings/SeriesAndEpisodesClients";
import SeriesRatingForm from "./SeriesRatingForm";
import { Container, Row, Col, Table } from "react-bootstrap";

interface IRouteProps {
  id: string;
}

interface IProps extends RouteComponentProps<IRouteProps> {
  client: ISeriesClient;
}

interface IState {
  series: ISeriesDetail | null;
}

export default class SeriesDetail extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      series: null,
    };
  }

  static defaultProps = {
    client: new SeriesClient(),
  };

  async componentDidMount() {
    const client: ISeriesClient = this.props.client;
    try {
      const response = await client.getSeries(this.props.match.params.id);
      this.setState({
        series: response,
      });
    } catch (err) {
      alert(err);
    }
  }

  render() {
    return (
      <>
        <h1>Series Detail</h1>
        <Link to={`${this.props.match.params.id}/edit`}>Edit</Link>
        {this.state.series ? (
          <Container>
            <Row>
              <Col>
                {this.state.series.title} (
                {this.state.series.firstAired.getFullYear()})
              </Col>
            </Row>
            <Row>
              <Col>
                <img
                  src={`/api/Images/${this.state.series.imageId}`}
                  alt="cover"
                ></img>
              </Col>
            </Row>
            <Row>
              <Col>
                Rating:
                <div>{this.state.series.averageRating} / 10</div>
              </Col>
              <Col>
                Description:
                <div>{this.state.series.description}</div>
              </Col>
            </Row>
            <h2>Episodes</h2>
            {this.state.series.seasons.length > 0 ? (
              <Row>
                <Col>
                  <Table striped bordered hover>
                    <thead>
                      <tr>
                        <th>Season</th>
                        <th>Episode</th>
                        <th>Title</th>
                        <th>Description</th>
                      </tr>
                    </thead>
                    <tbody>
                      {this.state.series.seasons.map((season) =>
                        season.episodes.map((episode) => (
                          <tr key={`${season.id}_${episode.id}`}>
                            <td>{season.id}</td>
                            <td>{episode.id}</td>
                            <td>{episode.title}</td>
                            <td>{episode.description}</td>
                          </tr>
                        ))
                      )}
                    </tbody>
                  </Table>
                </Col>
              </Row>
            ) : (
              <p>
                <em>No episodes added yet</em>
              </p>
            )}

            <Row>
              <Col>
                <SeriesRatingForm seriesId={this.state.series.id} />
              </Col>
            </Row>
          </Container>
        ) : (
          <p>
            <em>Loading...</em>
          </p>
        )}
      </>
    );
  }
}
