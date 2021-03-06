import { Field, Form, Formik } from "formik";
import React, { useContext } from "react";
import { Button, FormControl, FormGroup, FormLabel } from "react-bootstrap";
import Feedback from "react-bootstrap/Feedback";
import * as Yup from "yup";
import ClientsContext from "../ClientsContext";
import {
  ISeriesRatingsClient,
  SeriesRatingData,
  SeriesRatingsClient,
} from "../typings/RatingsClients";

const SeriesRatingForm = (props: {
  seriesId: string;
  client: ISeriesRatingsClient;
}) => {
  const clients = useContext(ClientsContext);
  const _client: ISeriesRatingsClient = clients.seriesRatingClient;

  return (
    <>
      <h1>Add rating</h1>
      <Formik
        initialValues={{
          opinion: "",
          rating: undefined,
          userId: undefined,
        }}
        validationSchema={Yup.object({
          opinion: Yup.string()
            .max(255, "Must be 255 characters or less")
            .required("Required"),
          rating: Yup.number()
            .min(1, "Must be at least 1")
            .max(10, "Must be less or equal to 10")
            .required("Required")
        })}
        onSubmit={async (values, { setSubmitting }) => {
          const request = new SeriesRatingData({
            opinion: values.opinion,
            rating: values.rating!!,
            seriesId: props.seriesId
          });
          try {
            const response = await _client.createSeriesRating(request);
            alert(JSON.stringify(response));
          } catch (err) {
            alert(err);
          }
        }}
      >
        {({ isSubmitting, touched, errors, isValid }) => (
          <Form noValidate>
            <FormGroup controlId="opinion">
              <FormLabel>Opinion</FormLabel>
              <Field
                as={FormControl}
                type="text"
                name="opinion"
                isInvalid={touched.opinion && errors.opinion}
              />
              <Feedback type="invalid">{errors.opinion}</Feedback>
            </FormGroup>
            <FormGroup controlId="rating">
              <FormLabel>Rating</FormLabel>
              <Field
                as={FormControl}
                type="number"
                name="rating"
                isInvalid={touched.rating && errors.rating}
              />
              <Feedback type="invalid">{errors.rating}</Feedback>
            </FormGroup>
            <Button variant="primary" type="submit" disabled={isSubmitting}>
              Submit
            </Button>
          </Form>
        )}
      </Formik>
    </>
  );
};

SeriesRatingForm.defaultProps = {
  client: new SeriesRatingsClient(),
};

export default SeriesRatingForm;
