import {Component, OnInit} from '@angular/core';
import {RdfDataService} from "../rdf-data.service";
import {DataSet} from 'vis';
import {VisNode} from "../data-model";

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
      console.log(result);

      const visNodes: VisNode[] = result.nodes.map(node => {
        let color;
        if (node.nodeType != 'Uri') {
          color = {background: '#FF8800', border: '#FF8800'};
        } else {
          color = {background: '#2A9FD6', border: '#2A9FD6'};
        }
        return {
          id: node.id,
          label: node.label,
          nodeType: node.nodeType,
          color: color
        };
      });

      this.graphData = {};
      this.graphData["nodes"] = new DataSet(visNodes);
      this.graphData["edges"] = new DataSet(result.edges);
    });
  }

}
