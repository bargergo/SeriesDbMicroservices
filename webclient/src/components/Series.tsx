import React from "react";
import { Link } from "react-router-dom";
import ClientsContext from "../ClientsContext";
import { FileResponse, IImageClient, ISeriesInfo } from "../typings/SeriesAndEpisodesClients";
import "./Series.css";

interface IState {
  image: FileResponse | null;
}

interface IProps {
  data: ISeriesInfo;
  path: string;
}

export default class Series extends React.Component<IProps, IState> {

  constructor(props: IProps) {
    super(props);
    this.state = {
      image: null
    };
  }

  static contextType = ClientsContext;

  async componentDidMount() {
    if (!this.props.data.imageId) {
      return;
    }
    const client: IImageClient = this.context.imageClient;
    try {
      const response = await client.getImage(this.props.data.imageId);
      this.setState(() => ({image: response}));
    } catch (err) {
      alert(err);
    }
  }

  render() {
    return (<>
      {!!this.state.image ? (<img
        className="cover-small"
        src={URL.createObjectURL(this.state.image.data)}
        alt="cover"
      ></img>) : (<p>loading image...</p>)}
      
      <Link to={`${this.props.path}/${this.props.data.id}`}>
        {this.props.data.title} ({this.props.data.firstAired.getFullYear()})
      </Link>
      {this.props.data.description}
    </>);
  }
}