import { Field, Form, Formik } from "formik";
import React from "react";
import { Button, FormControl, FormGroup, FormLabel } from "react-bootstrap";
import Feedback from "react-bootstrap/Feedback";
import * as Yup from "yup";
import {
  SeriesClient,
  UpsertSeriesRequest,
} from "../typings/SeriesAndEpisodesClients";

const SeriesDetailForm = () => {
  const client: SeriesClient = new SeriesClient();
  return (
    <>
      <h2>Details of the Series</h2>
      <Formik
        initialValues={{
          title: "",
          description: "",
          firstAired: "",
        }}
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
          const request = new UpsertSeriesRequest({
            title: values.title,
            description: values.description,
            firstAired: new Date(values.firstAired),
          });
          try {
            const response = await client.createSeries(request);
            alert(JSON.stringify(response));
          } catch (err) {
            alert(err);
          }
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
            <Button
              variant="primary"
              type="submit"
              disabled={isSubmitting || !isValid}
            >
              Submit
            </Button>
          </Form>
        )}
      </Formik>
    </>
  );
};

export default SeriesDetailForm;
