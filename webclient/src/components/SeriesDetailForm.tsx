import { Field, Form, Formik } from "formik";
import React, { Component } from "react";
import { Button, FormControl, FormGroup, FormLabel } from "react-bootstrap";
import Feedback from "react-bootstrap/Feedback";
import { RouteComponentProps } from "react-router-dom";
import * as Yup from "yup";
import {
  SeriesClient,
  UpsertSeriesRequest,
  ISeriesDetail,
  ISeriesClient,
} from "../typings/SeriesAndEpisodesClients";

interface IRouteProps {
  id: string | undefined;
}

interface IProps extends RouteComponentProps<IRouteProps> {
    client: ISeriesClient;
}

interface IState {
  series: ISeriesDetail | null;
  loading: boolean;
}

export default class SeriesDetailForm extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      series: null,
      loading: true,
    };
  }

  static defaultProps = {
    client: new SeriesClient()
  };

  async componentDidMount() {
    if (this.props.match.params.id) {
      const client: ISeriesClient = this.props.client;
      const series = await client.getSeries(this.props.match.params.id);
      this.setState({ series: series });
    }
    this.setState({ loading: false });
  }

  render() {
    const client: ISeriesClient = this.props.client;
    return (
      <>
        <h2>{this.props.match.params.id ? "Edit" : "Create"} Series</h2>
        {this.state.loading ? (
          <p>
            <em>Loading...</em>
          </p>
        ) : (
          <Formik
            initialValues={
              this.state.series
                ? {
                    title: this.state.series.title,
                    description: this.state.series.description,
                    firstAired: this.state.series.firstAired,
                  }
                : {
                    title: "",
                    description: "",
                    firstAired: "",
                  }
            }
            validationSchema={Yup.object({
              title: Yup.string()
                .max(15, "Must be 15 characters or less")
                .required("Required"),
              description: Yup.string()
                .max(255, "Must be 255 characters or less")
                .required("Required"),
              firstAired: Yup.date().required("Required"),
            })}
            onSubmit={async (values, { setSubmitting }) => {
              setSubmitting(true);
              const request = new UpsertSeriesRequest({
                title: values.title,
                description: values.description,
                firstAired: new Date(values.firstAired),
              });
              if (this.state.series) {
                try {
                  const response = await client.updateSeries(
                    this.state.series.id,
                    request
                  );
                  alert(JSON.stringify(response));
                } catch (err) {
                  alert(err);
                }
              } else {
                try {
                  const response = await client.createSeries(request);
                  alert(JSON.stringify(response));
                } catch (err) {
                  alert(err);
                }
              }
              setSubmitting(false);
            }}
          >
            {({ isSubmitting, touched, errors, isValid }) => (
              <Form noValidate>
                <FormGroup controlId="title">
                  <FormLabel>Title</FormLabel>
                  <Field
                    as={FormControl}
                    type="text"
                    name="title"
                    isInvalid={touched.title && errors.title}
                  />
                  <Feedback type="invalid">{errors.title}</Feedback>
                </FormGroup>
                <FormGroup controlId="description">
                  <FormLabel>Description</FormLabel>
                  <Field
                    as={FormControl}
                    type="text"
                    name="description"
                    isInvalid={touched.description && errors.description}
                  />
                  <Feedback type="invalid">{errors.description}</Feedback>
                </FormGroup>
                <FormGroup controlId="firstAired">
                  <FormLabel>First aired</FormLabel>
                  <Field
                    as={FormControl}
                    type="date"
                    name="firstAired"
                    isInvalid={touched.firstAired && errors.firstAired}
                  />
                  <Feedback type="invalid">{errors.firstAired}</Feedback>
                </FormGroup>
                <Button variant="primary" type="submit" disabled={isSubmitting}>
                  Submit
                </Button>
              </Form>
            )}
          </Formik>
        )}
      </>
    );
  }
}
