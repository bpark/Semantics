import {Component, OnInit, ViewChild, ElementRef} from '@angular/core';
import {RdfDataService} from "../rdf-data.service";
import {DataSet, Network, Options, Data, Node, Edge} from 'vis';
import {VisNode} from "../data-model";
import { Observable, Subject } from 'rxjs';

@Component({
  selector: 'app-rdf-graph',
  templateUrl: './rdf-graph.component.html',
  styleUrls: ['./rdf-graph.component.css']
})
export class RdfGraphComponent implements OnInit {

  @ViewChild('net') set networkElementRef(networkElementRef: ElementRef) {
    if (networkElementRef != undefined) {
      this.createNetwork(networkElementRef);
    }
  }

  graphData: Data;
  network: Network;

  constructor(private rdfDataService: RdfDataService) {}

  ngOnInit(): void {

    this.graphData = {};
    this.graphData["nodes"] = new DataSet();
    this.graphData["edges"] = new DataSet();

    this.queryData('Uri', 'permid:1-1003939166');
  }

  private createNetwork(networkElementRef: ElementRef): void {
    if (!this.network) {
      this.network = new Network(networkElementRef.nativeElement, this.graphData, this.createOptions());

      let gData = this.graphData;

      Observable.create((subject: Subject<VisNode>) => {

        this.network.on("click", function (params) {
          const selectedId = this.getNodeAt(params.pointer.DOM);
          let nodeData = gData['nodes'] as DataSet<Node>;
          nodeData.forEach(node => {
            if (node.id === selectedId) {
              let visNode = node as VisNode;;
              subject.next(visNode);
              subject.complete();
            }
          });
        });
      }).subscribe(result => {
        console.log(result);
        this.queryData(result.nodeType, result.label);
      });


    }
  }

  private onNetworkClick(node: VisNode): void {
    console.log(node);
    this.queryData(node.nodeType, node.label);
  }

  private queryData(nodeType: string, resource: string): void {
    this.rdfDataService.get(nodeType, resource).subscribe(result => {
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

      let nodeData = this.graphData["nodes"] as DataSet<Node>;
      let edgeData = this.graphData["edges"] as DataSet<Edge>;
      nodeData.add(visNodes);
      edgeData.add(result.edges);

      //this.graphData["nodes"] = new DataSet(visNodes);
      //this.graphData["edges"] = new DataSet(result.edges);
    });
  }

  private createOptions(): Options {
    const options = {
      edges: {
        //smooth: {
        //  forceDirection: "none"
        //},
        //font: {align: 'top'}
        font: {
          color: '#eeeeee',
          strokeWidth: 0
        }
      },
      physics: {
        barnesHut: {
          gravitationalConstant: -10200,
          springLength: 85
        },
        minVelocity: 0.75
      },
      nodes: {
        shape: 'dot',
        font: {
          color: '#eeeeee'
        }
        /*scaling: {
          label: {
            min: 8,
            max: 20
          }
        }*/
      },
      width: (window.innerWidth - 0) + "px",
      height: (window.innerHeight - 0) + "px"
    };

    return options;
  }

}
