import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import {
  ISeriesDetail,
  SeriesClient,
} from "../typings/SeriesAndEpisodesClients";
import SeriesRatingForm from "./SeriesRatingForm";
import { Container, Row, Col } from "react-bootstrap";

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
      <>
        <h1>Series Detail</h1>
        <Container>
          <Row>
            <Col>
              {this.state.title} ({this.state.firstAired.getFullYear()})
            </Col>
          </Row>
          <Row>
            <Col>
              <img src={`/api/Images/${this.state.imageId}`} alt="cover"></img>
            </Col>
          </Row>
          <Row>
            <Col>
              Rating:
              <div>{this.state.averageRating} / 10</div>
            </Col>
            <Col>
              Description:
              <div>{this.state.description}</div>
            </Col>
          </Row>
          <Row>
            <Col>
              <SeriesRatingForm seriesId={this.state.id} />
            </Col>
          </Row>
        </Container>
      </>
    );
  }
}

export default SeriesDetail;
