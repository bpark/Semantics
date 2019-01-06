import { Component, OnInit } from '@angular/core';
import {RdfDataService} from "../rdf-data.service";
import { DataSet } from 'vis';

@Component({
  selector: 'app-rdf-graph',
  templateUrl: './rdf-graph.component.html',
  styleUrls: ['./rdf-graph.component.css']
})
export class RdfGraphComponent implements OnInit {

  title = 'semantics-ui';

  graphData;

  constructor(private rdfDataService: RdfDataService) {}

  ngOnInit(): void {
    this.rdfDataService.get().subscribe(result => {
      // provide the data in the vis format
      console.log(result);
      this.graphData = {};
      this.graphData["nodes"] = new DataSet(result.nodes);
      this.graphData["edges"] = new DataSet(result.edges);
    });
  }

}
