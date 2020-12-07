import React, { Component } from "react";
import { Col, Container, Row, Table } from "react-bootstrap";
import { Link, RouteComponentProps } from "react-router-dom";
import ClientsContext from "../ClientsContext";
import {
  FileResponse,
  IImageClient,
    ISeriesClient,
    ISeriesDetail
} from "../typings/SeriesAndEpisodesClients";
import SeriesRatingForm from "./SeriesRatingForm";

interface IRouteProps {
  id: string;
}

interface IProps extends RouteComponentProps<IRouteProps> {
}

interface IState {
  series: ISeriesDetail | null;
  image: FileResponse | null;
}

export default class SeriesDetailComponent extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      series: null,
      image: null
    };
  }

  static contextType = ClientsContext;

  async componentDidMount() {
    const client: ISeriesClient = this.context.seriesClient;
    const imageClient: IImageClient = this.context.imageClient;
    try {
      const response = await client.getSeries(this.props.match.params.id);
      this.setState({
        series: response,
      });
      const imageResponse = await imageClient.getImage(response.imageId);
      this.setState(() => ({image: imageResponse}));
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
                  {!!this.state.image ? (<img
                    src={URL.createObjectURL(this.state.image.data)}
                    alt="cover"
                  ></img>) : (<p>loading image...</p>)}
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
